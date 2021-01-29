using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LilWidgets.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressWidget : ContentView
    {
        #region Constants

        const float SWEEP_START = -90;
        const float ALLOWED_DEVIATION = 0.009f;
        public const float DEFAULT_ANIMATION_DURATION = 1000;
        public const float DEFAULT_FRAME_RATE = 60f;
        public const float DEFAULT_STROKE_WIDTH = 20;
        public const float DEFAULT_ARC_TO_TEXT_SPACING = 10;
        readonly float cycleTime = 1f / DEFAULT_FRAME_RATE;

        #endregion Constants

        #region Bindable Properties

        /// <summary>
        /// Bindable Property for the <see cref="PercentValue"/> property.
        /// </summary>
        public static readonly BindableProperty PercentValueProperty = BindableProperty.Create(nameof(PercentValue), typeof(double), typeof(ProgressWidget), 0d, BindingMode.OneWay, ValidatePercentage, PercentValuePropertyChanged);

        public static readonly BindableProperty BackRingColorProperty = BindableProperty.Create(nameof(BackArcColor), typeof(Color), typeof(ProgressWidget), Color.Black, BindingMode.OneWay, null, BackgroundRingColorPropertyChanged);

        public static readonly BindableProperty ProgressRingColorProperty = BindableProperty.Create(nameof(ProgressArcColor), typeof(Color), typeof(ProgressWidget), Color.White, BindingMode.OneWay, null, ProgressRingColorPropertyChanged);

        public static readonly BindableProperty IsTextEnabledProperty = BindableProperty.Create(nameof(IsTextEnabled), typeof(bool), typeof(ProgressWidget), true, BindingMode.OneWay, null, IsTextEnabledPropertyChanged);

        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(float), typeof(ProgressWidget), DEFAULT_ANIMATION_DURATION);

        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(ProgressWidget), DEFAULT_STROKE_WIDTH, BindingMode.OneWay, null, StokeWidthPropertyChanged);

        public static readonly BindableProperty ArcToTextSpacingProperty = BindableProperty.Create(nameof(ArcToTextSpacing), typeof(float), typeof(ProgressWidget), DEFAULT_ARC_TO_TEXT_SPACING, BindingMode.OneWay, null, ArcToTextSpacingPropertyChanged);
        #endregion Bindable Properties

        #region Properties

        /// <summary>
        /// Contains the value of progress. Expects a value from 0.0 to 1.0 representing the progress.
        /// </summary>
        public double PercentValue
        {
            get => (double)GetValue(PercentValueProperty);
            set => SetValue(PercentValueProperty, value);
        }

        public Color BackArcColor
        {
            get => (Color)GetValue(BackRingColorProperty);
            set => SetValue(BackRingColorProperty, value);
        }

        public Color ProgressArcColor
        {
            get => (Color)GetValue(ProgressRingColorProperty);
            set => SetValue(ProgressRingColorProperty, value);
        }

        public bool IsTextEnabled
        {
            get => (bool)GetValue(IsTextEnabledProperty);
            set => SetValue(IsTextEnabledProperty, value);
        }

        public float Duration
        {
            get => (float)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public float StrokeWidth
        {
            get => (float)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }

        public float ArcToTextSpacing
        {
            get => (float)GetValue(ArcToTextSpacingProperty);
            set => SetValue(ArcToTextSpacingProperty, value);
        }

        public int MyProperty { get; set; }

        private bool animating;

        public bool Animating
        {
            get => animating;
            private set
            {
                if (animating == value) return;
                animating = value;
#if DEBUG
                if (animating)
                    debugWatch.Restart();
                else
                {
                    debugWatch.Stop();
                    Debug.WriteLine($"Animation Finished | Target Duration: {millisecondDuration} (milli) | Actual: {debugWatch.ElapsedMilliseconds} (milli). " +
                        $"Error Percentage: {Math.Abs(millisecondDuration - debugWatch.ElapsedMilliseconds) / millisecondDuration:P}");
                }
#endif
            }
        }

        #endregion Properties

        #region Fields

        SKPaint progressPaint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        SKPaint backgroundPaint = new SKPaint
        {
            Color = SKColors.LightGray,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 4, 4, SKColor.Parse("#5555"))
        };

        /// <summary>
        /// Used to determine when the animation should stop
        /// </summary>
        Func<bool> comparer = null;
     

        //double nValue = 0;
        //double oValue = 0;
        float millisecondDuration = DEFAULT_ANIMATION_DURATION; // 2 seconds
        double difference = 0;
        /// <summary>
        /// Used to store the result of <see cref="SKPaint.MeasureText(string, ref SKRect)"/>.
        /// Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float textWidth = 1;
        /// <summary>
        /// Used to store the animations current progress towards the <see cref="PercentValue"/>.
        /// Updates come the <see cref="Device.StartTimer(TimeSpan, Func{bool})"/> when <see cref="PercentValuePropertyChanged(BindableObject, object, object)"/> is invoked.
        /// </summary>
        double currentPercentageValue = 0;
        /// <summary>
        /// Contains the mid x value of the <see cref="canvas"/>. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float midX = 0;
        /// <summary>
        /// Contains the mid y value of the <see cref="canvas"/>. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float midY = 0;
        /// <summary>
        /// Used in conjunction with the <see cref="Device.StartTimer(TimeSpan, Func{bool})"/>'s lambda inside the <see cref="PercentValuePropertyChanged(BindableObject, object, object)"/>
        /// to determine the amount of time passed from the last time being restarted. This is used as deltaTime to help smooth out animations and pull away from using framerate as a method of
        /// determining how much a target value should change over a given amount of time.
        /// </summary>
        Stopwatch stopwatch = null;
        /// <summary>
        /// Contains the last determined value of <see cref="StrokeWidth"/> * <see cref="strokeRatio"/> / 2.
        /// Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float halfOfRelativeStrokeWidth = 0;
        /// <summary>
        /// Contains the bounds of both arcs and is a factor when determining the text's size.
        /// Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        SKRect arcRect;
        /// <summary>
        /// Contains information about the <see cref="canvas"/> from the last use of <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/>.
        /// </summary>
        SKImageInfo info;
        /// <summary>
        /// Contains the last determined value of the product of <see cref="StrokeWidth"/> * <see cref="strokeRatio"/>.
        /// This is used for resizing. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float relativeStrokeWidth = 1;
        /// <summary>
        /// A string that contains the last percentage message displayed to the user if <see cref="IsTextEnabled"/> is true.
        /// Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        string percentageMsg = string.Empty;
        /// <summary>
        /// Stores a ratio of the canvas size and <see cref="StrokeWidth"/> to be used when resizing. 
        /// This value only updates when the <see cref="StrokeWidth"/> is changed.
        /// </summary>
        float strokeRatio = 1;
        /// <summary>
        /// Stores a ratio of the canvas size and the <see cref="ArcToTextSpacing"/> to be used when resizing.
        /// This value only updates when the <see cref="ArcToTextSpacing"/> is changed.
        /// </summary>
        float txtSpaceRatio = 1;

#if DEBUG
        /// <summary>
        /// Used for debugging.
        /// </summary>
        private Stopwatch debugWatch = new Stopwatch();
#endif
        #endregion Fields

        public ProgressWidget()
        {
            InitializeComponent();
        }        

        private void canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            info = e.Info;

            canvas.Clear(SKColors.LightBlue);

            //canvas.DrawColor(SKColors.LightBlue);
            midX = info.Rect.MidX;
            midY = info.Rect.MidY;
            relativeStrokeWidth = StrokeWidth * strokeRatio;
            halfOfRelativeStrokeWidth = relativeStrokeWidth / 2;

            // Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            if (midX > midY) // Cavnas is wider than it is tall, hence computer for height
            {
                arcRect = new SKRect(info.Width / 2f - info.Height / 2f + halfOfRelativeStrokeWidth, // left
                                     halfOfRelativeStrokeWidth, // top
                                     info.Width / 2f + info.Height / 2f - halfOfRelativeStrokeWidth, // right
                                     info.Height - halfOfRelativeStrokeWidth); // bottom
            }
            else // Canvas is taller than it is wide to compute for width
            {                
                arcRect = new SKRect(halfOfRelativeStrokeWidth, // left
                                     info.Height / 2f - info.Width / 2f + halfOfRelativeStrokeWidth, // top
                                     info.Width - halfOfRelativeStrokeWidth, // right
                                     info.Height / 2f + info.Width / 2f - halfOfRelativeStrokeWidth); // bottom
            }
            //arcRect.Location = new SKPoint(0, centerYOffset - centerXOffset / 2); // Centering to the center of the parent view x and y

            //float sweepAngle = 0;// = 75f / 100f * 360f;  // (90 / 100) * 360 -- percentage to drawing angle
            //PercentageToSweepAngle(sweepAngle);
            //canvas.DrawCircle(new SKPoint(centerXOffset, centerYOffset), centerXOffset, paint);
            SKPath progressPath = new SKPath();
            progressPath.AddArc(arcRect, SWEEP_START, PercentageToSweepAngle(currentPercentageValue));
            SKPath backgroundPath = new SKPath();
            backgroundPath.AddArc(arcRect, SWEEP_START, 360f);

            //relativeStroke = StrokeWidth * strokeRatio;

            progressPaint.StrokeWidth = relativeStrokeWidth;
            backgroundPaint.StrokeWidth = relativeStrokeWidth;
            // Draw Calls
            canvas.DrawPath(backgroundPath, backgroundPaint); // Background Arc
            canvas.DrawPath(progressPath, progressPaint); // Progress Arc

            if (IsTextEnabled) // Draw text only if enabled
            {
                SKPaint textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true
                };

                percentageMsg = currentPercentageValue.ToString("P");

                // Adjust TextSize property so text is 75% of screen width
                textWidth = textPaint.MeasureText(percentageMsg);
                textPaint.TextSize = (arcRect.Width - relativeStrokeWidth - ArcToTextSpacing) * textPaint.TextSize / textWidth * txtSpaceRatio;// - (strokeWidth * 2);

                // Find the text bounds
                SKRect textBounds = new SKRect();// (strokeWidth, strokeWidth, rect.Width - strokeWidth, rect.Height - strokeWidth);
                textPaint.MeasureText(percentageMsg, ref textBounds);

                // Calculate offsets to center the text on the screen
                float xText = arcRect.MidX - textBounds.MidX;
                float yText = arcRect.MidY - textBounds.MidY;

                canvas.DrawText(percentageMsg, xText, yText, textPaint); // Progress Text
            }            
        }

        private float PercentageToSweepAngle(double percentage)
            => (float)percentage * 100 / 100f * 360f;

        /// <summary>
        /// Validates the given value of the <see cref="PercentValue"/> property.
        /// </summary>
        /// <param name="bindable">Instance of declaring type.</param>
        /// <param name="value">Value of <see cref="PercentValue"/></param>
        /// <returns>Indication if successful validation.</returns>
        private static bool ValidatePercentage(BindableObject bindable, object value)
        {
            double? percentage = value as double?;

            if (percentage == null)
                return false;
            else if (percentage < 0 || percentage > 1)
                return false;

            return true;
        }

        /// <summary>
        /// A callback function to the <see cref="PercentValue"/> being changed. IMPORTANT: No variables that are used directly inside the <see cref="Device.StartTimer(TimeSpan, Func{bool})"/>'s lambda can be instantiated here.
        /// This is because the closure that is generated by the lambda must capture references to variables that can be modified from the owning class instance. This means the class can modify the closure's 
        /// reference variables and therefore alter its behavior which is required.
        /// </summary>
        /// <param name="bindable">Declaring type.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">Newly assigned value.</param>
        private static void PercentValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            double nValue = (double)newValue;
            double oValue = (double)oldValue;
            widget.millisecondDuration = widget.Duration * (float)Math.Abs(nValue - oValue);

            // Get the signed change needed each frame
            widget.difference = (nValue - oValue);// / widget.totalFrames;

            // No use of locking variables for thread safety should be needed because all modifications from timer are queue in the mainthread dispatcher

            if (widget.difference < 0) // If decreasing
                widget.comparer = () => widget.currentPercentageValue > widget.PercentValue + ALLOWED_DEVIATION; // When decreasing, if the value is larger than the target keep decreasing.
            else // If inreasing
                widget.comparer = () => widget.currentPercentageValue < widget.PercentValue - ALLOWED_DEVIATION; // When increasing, if the value is smaller than the target keep increasing.

            if (widget.Animating) // Prevent another timer being started if the bindable widget is already animating itself
                return;
            else // If the widget animating state is false then assign true because we shall start the animation
                widget.Animating = true;

            double relativeDuration = 0;

            widget.stopwatch = new Stopwatch();
            widget.stopwatch.Start();           
            Device.StartTimer(TimeSpan.FromSeconds(1f / DEFAULT_FRAME_RATE), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    widget.stopwatch.Stop();

                    // We want to take a percentage of the duration that is relative to the percentage of the total we are moving
                    // Therefore moving from 0 to 50% will take 50% of the Duration propertys value in time
                    relativeDuration = widget.Duration / 1000 * Math.Abs(widget.difference);

                    // Add the correct difference based with respects the desired duration then multiplied by the time passed 
                    widget.currentPercentageValue += widget.difference / relativeDuration * widget.stopwatch.Elapsed.TotalSeconds;
                    widget.stopwatch.Restart();
                    // Informing the SKCanvasView that it must redraw itself
                    widget.canvas.InvalidateSurface();
#if DEBUG
                    double beforeDelta = widget.difference / (widget.Duration / 1000);
                    Debug.WriteLine($"currentPercentageValue: {widget.currentPercentageValue} | Target: {widget.PercentValue} || " +
                        $"Target Cycle Time: {widget.cycleTime} (milli) | Actual: {widget.stopwatch.ElapsedMilliseconds} (milli) | " +
                        $"Correction Percentage: {Math.Abs(beforeDelta - widget.currentPercentageValue) / beforeDelta:P}");
#endif
                    widget.Animating = widget.comparer.Invoke();

                    if (!widget.Animating) // If not animating anymore assign the currentPercentageValue the exact desired amount
                        widget.currentPercentageValue = widget.PercentValue;
                });

                // Continue while we haven't reached the target value
                return widget.Animating;
            });
        }

        private static void BackgroundRingColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.backgroundPaint.Color = ((Color)newValue).ToSKColor();
            TryUpdate(widget);
        }

        private static void ProgressRingColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.progressPaint.Color = ((Color)newValue).ToSKColor();
            TryUpdate(widget);
        }

        private static void IsTextEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => TryUpdate((ProgressWidget)bindable);      

        private static void StokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (ProgressWidget)bindable;

            widget.strokeRatio = (widget.arcRect.Width - widget.StrokeWidth) / widget.arcRect.Width;

            TryUpdate(widget);
        }       

        private static void ArcToTextSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (ProgressWidget)bindable;
            widget.txtSpaceRatio = (widget.arcRect.Width - widget.ArcToTextSpacing) / widget.arcRect.Width;
            TryUpdate(widget);
        }

        private static void TryUpdate(ProgressWidget widget)
        {            
            if (!widget.Animating) // Dont update if we are animating because the next frame will be updated anyway
                widget.canvas.InvalidateSurface();
        }

    }
}