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

        const float ALLOWED_DEVIATION = 0.009f;
        const float DEFAULT_ANIMATION_DURATION = 1000;
        const float DEFAULT_FRAME_RATE = 60f;
        const float DEFAULT_STROKE_WIDTH = 20;
        readonly float cycleTime = 1f / DEFAULT_FRAME_RATE;

        #endregion Constants

        #region Bindable Properties

        /// <summary>
        /// Bindable Property for the <see cref="PercentValue"/> property.
        /// </summary>
        public static readonly BindableProperty PercentValueProperty = BindableProperty.Create(nameof(PercentValue), typeof(double), typeof(ProgressWidget), 0d, BindingMode.OneWay, ValidatePercentage, PercentValuePropertyChanged);

        public static readonly BindableProperty BackRingColorProperty = BindableProperty.Create(nameof(BackRingColor), typeof(Color), typeof(ProgressWidget), Color.Black, BindingMode.OneWay, null, BackgroundRingColorPropertyChanged);

        public static readonly BindableProperty ProgressRingColorProperty = BindableProperty.Create(nameof(ProgressRingColor), typeof(Color), typeof(ProgressWidget), Color.White, BindingMode.OneWay, null, ProgressRingColorPropertyChanged);

        public static readonly BindableProperty IsTextEnabledProperty = BindableProperty.Create(nameof(IsTextEnabled), typeof(bool), typeof(ProgressWidget), true, BindingMode.OneWay, null, IsTextEnabledPropertyChanged);

        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(float), typeof(ProgressWidget), DEFAULT_ANIMATION_DURATION);

        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(ProgressWidget), DEFAULT_STROKE_WIDTH);

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

        public Color BackRingColor
        {
            get => (Color)GetValue(BackRingColorProperty);
            set => SetValue(BackRingColorProperty, value);
        }

        public Color ProgressRingColor
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

        private SKPaint progressPaint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        private SKPaint backgroundPaint = new SKPaint
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
        private Func<bool> comparer = null;

        //private SKPaint textPaint = new SKPaint
        //{
        //    Color = SKColors.Blue,
        //    Style = SKPaintStyle.Fill,
        //    IsAntialias = true
        //};

        private double nValue = 0;
        private double oValue = 0;
        private float millisecondDuration = DEFAULT_ANIMATION_DURATION; // 2 seconds

        // Get the signed change needed each frame
        private double difference = 0;

        private float textWidth = 1;
        private double currentPercentageValue = 0;
        private float centerXOffset = 0;
        private float centerYOffset = 0;
        private float sweepStart = -90;
        private Stopwatch stopwatch = null;

#if DEBUG
        private Stopwatch debugWatch = new Stopwatch();
#endif

        #endregion Fields

        public ProgressWidget()
        {
            InitializeComponent();
        }

        float halfOfStrokeWidth = 0;
        float top = 0;
        float bottom = 0;
        SKRect arcRect;
        SKImageInfo info;

        private void canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            info = e.Info;

            canvas.Clear(SKColors.LightBlue);

            //canvas.DrawColor(SKColors.LightBlue);

            centerXOffset = info.Width / 2;
            centerYOffset = info.Height / 2;

            halfOfStrokeWidth = StrokeWidth / 2;

            // Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            if (centerXOffset > centerYOffset) // Cavnas is wider than it is tall, hence computer for height
            {
                arcRect = new SKRect(info.Width / 2f - info.Height / 2f + halfOfStrokeWidth, // left
                                     halfOfStrokeWidth, // top
                                     info.Width / 2f + info.Height / 2f - halfOfStrokeWidth, // right
                                     info.Height - halfOfStrokeWidth); // bottom
            }
            else // Canvas is taller than it is wide to compute for width
            {                
                arcRect = new SKRect(halfOfStrokeWidth, // left
                                     info.Height / 2f - info.Width / 2f + halfOfStrokeWidth, // top
                                     info.Width - halfOfStrokeWidth, // right
                                     info.Height / 2f + info.Width / 2f - halfOfStrokeWidth); // bottom
            }
            //arcRect.Location = new SKPoint(0, centerYOffset - centerXOffset / 2); // Centering to the center of the parent view x and y

            //float sweepAngle = 0;// = 75f / 100f * 360f;  // (90 / 100) * 360 -- percentage to drawing angle
            //PercentageToSweepAngle(sweepAngle);
            //canvas.DrawCircle(new SKPoint(centerXOffset, centerYOffset), centerXOffset, paint);
            SKPath progressPath = new SKPath();
            progressPath.AddArc(arcRect, sweepStart, PercentageToSweepAngle(currentPercentageValue));
            SKPath backgroundPath = new SKPath();
            backgroundPath.AddArc(arcRect, sweepStart, 360f);

            progressPaint.StrokeWidth = StrokeWidth;
            backgroundPaint.StrokeWidth = StrokeWidth;
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

                string str = currentPercentageValue.ToString("P");

                // Adjust TextSize property so text is 75% of screen width
                textWidth = textPaint.MeasureText(str);
                textPaint.TextSize = textWidth;// - (strokeWidth * 2);

                // Find the text bounds
                SKRect textBounds = new SKRect();// (strokeWidth, strokeWidth, rect.Width - strokeWidth, rect.Height - strokeWidth);
                var test = textPaint.MeasureText(str, ref textBounds);

                canvas.DrawText(str, arcRect.MidX - textBounds.Width / 2, arcRect.MidY - textBounds.Height / 2, textPaint); // Progress Text
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
        /// A callback function to the <see cref="PercentValue"/> being changed.
        /// </summary>
        /// <param name="bindable">Declaring type.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">Newly assigned value.</param>
        private static void PercentValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.nValue = (double)newValue;
            widget.oValue = (double)oldValue;
            widget.millisecondDuration = widget.Duration * (float)Math.Abs(widget.nValue - widget.oValue);

            // Get the signed change needed each frame
            widget.difference = (widget.nValue - widget.oValue);// / widget.totalFrames;

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
            widget.TryUpdate();
        }

        private static void ProgressRingColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.progressPaint.Color = ((Color)newValue).ToSKColor();
            widget.TryUpdate();
        }

        private static void IsTextEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.TryUpdate();
        }

        private void TryUpdate()
        {
            if (!Animating) // Dont update if we are animating because the next frame will be updated anyway
                canvas.InvalidateSurface();
        }
    }
}