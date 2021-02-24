using LilWidgets.Lang;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LilWidgets.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingWidget : ContentView
    {
        #region Constants
        /// <summary>
        /// The base sigma for the drop shadow used with the backing arc.
        /// </summary>
        const float BASE_SHADOW_SIGMA = 3f;
        /// <summary>
        /// The starting position of the arcs.
        /// </summary>
        const float SWEEP_START = -90;
        /// <summary>
        /// The allowed deviation for the progress animation.
        /// </summary>
        const float ALLOWED_DEVIATION = 0.009f;
        /// <summary>
        /// The default value for the duration of the progress animation.
        /// </summary>
        public const float DEFAULT_ANIMATION_DURATION = 2000;
        /// <summary>
        /// The default frame-rate being targeted.
        /// </summary>
        public const float DEFAULT_FRAME_RATE = 60f;
        /// <summary>
        /// The default stroke width used for the arcs.
        /// </summary>
        public const float DEFAULT_STROKE_WIDTH = 10;
        /// <summary>
        /// The default cycle time to be aimed for.
        /// </summary>
        readonly float cycleTime = 1f / DEFAULT_FRAME_RATE;
        /// <summary>
        /// The default shadow color used with the arcs.
        /// </summary>
        public static readonly Color defaultShadowColor = Color.FromHex("#5555");
        #endregion Constants

        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="BackArcColorProperty"/> property.
        /// </summary>
        public static readonly BindableProperty BackArcColorProperty = BindableProperty.Create(nameof(BackArcColor), typeof(Color), typeof(ProgressWidget), Color.Black, BindingMode.OneWay, null, BackgroundRingColorPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="Duration"/> property.
        /// </summary>
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(float), typeof(ProgressWidget), DEFAULT_ANIMATION_DURATION);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(ProgressWidget), DEFAULT_STROKE_WIDTH, BindingMode.OneWay, null, StokeWidthPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ShadowColor"/> property.
        /// </summary>
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(ProgressWidget), defaultShadowColor, BindingMode.OneWay, null, ShadowColorPropertyChanged);
        #endregion Bind-able Properties

        #region Properties
        /// <summary>
        /// The color of the backing arc to the progress arc.
        /// </summary>
        public Color BackArcColor
        {
            get => (Color)GetValue(BackArcColorProperty);
            set => SetValue(BackArcColorProperty, value);
        }
        /// <summary>
        /// The color of the shadow to be used with the arcs.
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
        /// <summary>
        /// The time in milliseconds it takes for 1 complete cycle of the animation.
        /// </summary>
        public float Duration
        {
            get => (float)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }
        /// <summary>
        /// The target stroke width value to be used for all arcs that make up the widget.
        /// </summary>
        public float StrokeWidth
        {
            get => (float)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }
        private bool animating;
        /// <summary>
        /// Indicates whether the progress widget is animating right now.
        /// </summary>
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
                    Debug.WriteLine($"Animation Finished | Target Duration: {millisecondDuration} (milliseconds) | Actual: {debugWatch.ElapsedMilliseconds} (milliseconds) | " +
                        $"Error Percentage: {Math.Abs(millisecondDuration - debugWatch.ElapsedMilliseconds) / millisecondDuration:P}");
                }
#endif
            }
        }
        #endregion Properties

        #region Fields
        /// <summary>
        /// Used to draw the background arc.
        /// </summary>
        SKPaint backgroundPaint = new SKPaint
        {
            Color = SKColors.White,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, BASE_SHADOW_SIGMA, BASE_SHADOW_SIGMA, defaultShadowColor.ToSKColor())
        };
        /// <summary>
        /// Indicates whether the <see cref="strokeRatio"/> needs to be re-calculated.
        /// </summary>
        bool isStrokeRatioDirty = true;
        /// <summary>
        /// The target duration of the progress animation in milliseconds.
        /// </summary>
        float millisecondDuration = DEFAULT_ANIMATION_DURATION; // 2 seconds
        /// <summary>
        /// The mid-x value of the <see cref="canvas"/>. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float midX = 0;
        /// <summary>
        /// The mid-y value of the <see cref="canvas"/>. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float midY = 0;
        /// <summary>
        /// Used in conjunction with the <see cref="Device.StartTimer(TimeSpan, Func{bool})"/>'s lambda inside the <see cref="PercentValuePropertyChanged(BindableObject, object, object)"/>
        /// to determine the amount of time passed from the last time being restarted. This is used as deltaTime to help smooth out animations and pull away from using frame-rate as a method of
        /// determining how much a target value should change over a given amount of time.
        /// </summary>
        Stopwatch stopwatch = null;
        /// <summary>
        /// The last determined value of <see cref="StrokeWidth"/> * <see cref="strokeRatio"/> / 2.
        /// Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float halfOfRelativeStrokeWidth = 0;
        /// <summary>
        /// The bounds or frame of both arcs and is a factor when determining the text's size.
        /// Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        SKRect arcRect;
        /// <summary>
        /// The information about the <see cref="canvas"/> from the last use of <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/>.
        /// </summary>
        SKImageInfo info;
        /// <summary>
        /// The last determined value of the product of <see cref="StrokeWidth"/> * <see cref="strokeRatio"/>.
        /// This is used for resizing. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float relativeStrokeWidth = 1;
        /// <summary>
        /// The ratio of the canvas size and <see cref="StrokeWidth"/> to be used when resizing. 
        /// This value only updates when the <see cref="StrokeWidth"/> is changed.
        /// </summary>
        float strokeRatio = 0.15f;
        /// <summary>
        /// Indicates whether the width of the <see cref="canvas"/> is greater than the height.
        /// </summary>
        bool isWidthGreaterThanHeight = false;
        /// <summary>
        /// The smallest span of the <see cref="canvas"/>. This can either be the span from the top to the bottom aka the height
        /// or the left to the right aka the width.
        /// </summary>
        //float limitingSpan = 0;
        /// <summary>
        /// The shadow sigma used for the drop shadow which is relative to the <see cref="strokeRatio"/>. This helps the shadow resize correctly. Without this the
        /// shadow can be come obnoxious when scaling from a larger size to a smaller one.
        /// </summary>
        float relativeShadowSigma = 0;
        /// <summary>
        /// The color used for the drop shadow.
        /// </summary>
        SKColor shadowColor = defaultShadowColor.ToSKColor();
        /// <summary>
        /// The width of half the stroke of the shadow arc. This is used as a padding when determining <see cref="arcRect"/> because the shadow is the widest
        /// arc compared to the others.
        /// </summary>
        float halfShadowStrokeWidth = 0;
        /// <summary>
        /// The path used to define the background arc.
        /// </summary>
        SKPath backgroundPath = null;
        /// <summary>
        /// The current sweep position in the animation.
        /// </summary>
        float sweepPercentage = 0;
#if DEBUG
        /// <summary>
        /// Used for debugging.
        /// </summary>
        private Stopwatch debugWatch = new Stopwatch();
#endif
        #endregion Fields

        /// <summary>
        /// Primary Constructor.
        /// </summary>
        public LoadingWidget()
        {
            InitializeComponent();
            limitingSpan = new LimitingSpan(this);
        }      

        private LimitingSpan limitingSpan;

        /// <summary>
        /// Applies the desired graphics to the <see cref="canvas"/>.
        /// </summary>
        private void canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            info = e.Info;             

            canvas.Clear();

            midX = info.Rect.MidX;
            midY = info.Rect.MidY;

            isWidthGreaterThanHeight = info.Width > info.Height;
            //limitingSpan = (isWidthGreaterThanHeight ? info.Height : info.Width);

            if (isStrokeRatioDirty) // Calculate the correct strokeRatio if needed
                UpdateStrokeRatio();

            relativeStrokeWidth = limitingSpan.SpanWidthInPixels * strokeRatio;
            halfOfRelativeStrokeWidth = relativeStrokeWidth / 2;
            relativeShadowSigma = BASE_SHADOW_SIGMA + BASE_SHADOW_SIGMA * strokeRatio;
            // Compensate for the shadow
            halfShadowStrokeWidth = halfOfRelativeStrokeWidth + relativeShadowSigma * 3f;

            // Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            if (isWidthGreaterThanHeight) // Canvas is wider than it is tall, hence computer for height
            {
                arcRect = new SKRect(midX - midY + halfShadowStrokeWidth, // left
                                     halfShadowStrokeWidth, // top
                                     midX + midY - halfShadowStrokeWidth, // right
                                     info.Height - halfShadowStrokeWidth); // bottom
            }
            else // Canvas is taller than it is wide so compute for width
            {
                arcRect = new SKRect(halfShadowStrokeWidth, // left
                                     midY - midX + halfShadowStrokeWidth, // top
                                     info.Width - halfShadowStrokeWidth, // right
                                     midY + midX - halfShadowStrokeWidth); // bottom
            }

            if (arcRect.Width < 0 || arcRect.Height < 0) // return if the control is becoming negatively sized
                return;
            if (relativeStrokeWidth > arcRect.Width)
                return;
#if DEBUG
            //throw new Exception($"Error. Invalid stroke width was given. The stroke width {StrokeWidth} is larger than the view can handle (strokeWidth * 2 > totalWidth == true).");
            Debug.WriteLine("Degrees: " + PercentageToSweepAngle(sweepPercentage));
            //Debug.WriteLine($"Stroke Width: {StrokeWidth} | Stroke Ratio: {strokeRatio} | Relative Stroke: {relativeStrokeWidth}");
#endif
            // Set the shadow for the background arc
            backgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(0,
                                                                         0,
                                                                         relativeShadowSigma,
                                                                         relativeShadowSigma,
                                                                         shadowColor);

            // Creating paths
            backgroundPath = new SKPath();
            backgroundPath.AddArc(arcRect, PercentageToSweepAngle(sweepPercentage), 90);
            
            // Applying path widths aka strike widths
            backgroundPaint.StrokeWidth = relativeStrokeWidth;
            // Draw Calls
            canvas.DrawPath(backgroundPath, backgroundPaint); // Background Arc
        }

        private void Animate()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
#if DEBUG
            debugWatch.Start();
#endif            
            Device.StartTimer(TimeSpan.FromSeconds(1f / DEFAULT_FRAME_RATE), () =>
            {
                float dur = 1 / (Duration / 1000);

                //(float)percentage * 100 / 100f * 360f;

                Device.BeginInvokeOnMainThread(() =>
                {
                    stopwatch.Stop();

                    // We want to take a percentage of the duration that is relative to the percentage of the total we are moving
                    // Therefore moving from 0 to 50% will take 50% of the Duration properties value in time
                    //relativeDuration = Duration / 1000 * Math.Abs(difference);
                    //relativeDuration = widget.Duration / 1000 * Math.Abs(widget.difference);

                    // Add the correct difference based with respects the desired duration then multiplied by the time passed 
                    //widget.currentPercentageValue += widget.difference / relativeDuration * widget.stopwatch.Elapsed.TotalSeconds;
                    // Add the correct difference based with respects the desired duration then multiplied by the time passed 
                    sweepPercentage += dur * (float)stopwatch.Elapsed.TotalSeconds;
                    stopwatch.Restart();

#if DEBUG
                    Debug.WriteLine($"Sweep Position: {sweepPercentage} | Time Stamp: {debugWatch.ElapsedMilliseconds}");
                    
#endif

                    // Informing the SKCanvasView that it must redraw itself
                    canvas.InvalidateSurface();
                });

                // Continue while we haven't reached the target value
                return Animating;
                //return sweepPercentage < 1;
            });           
        }        

        public void StartAnimating()
        {
            // No use of locking variables for thread safety should be needed because all modifications from timer are queue in the main-thread dispatcher
            if (Animating) // Prevent another timer being started if the bind-able widget is already animating itself
                return;
            else // If the widget animating state is false then assign true because we shall start the animation
            {
                Animating = true;
                Animate();
            }
        }

        public void StopAnimating()
        {
            if (!Animating)
                return;
            else
                Animating = false;
        }

        private float PercentageToSweepAngle(double percentage)
          => (float)percentage * 100 / 100f * 360f;

        #region Property Change Handlers
        private static void BackgroundRingColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (LoadingWidget)bindable;
            widget.backgroundPaint.Color = ((Color)newValue).ToSKColor();
            TryUpdate(widget);
        }
        private static void StokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (LoadingWidget)bindable;
            widget.isStrokeRatioDirty = true;
            TryUpdate(widget);
        }
        private static void ShadowColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (LoadingWidget)bindable;
            widget.shadowColor = ((Color)newValue).ToSKColor();
            TryUpdate(widget);
        }
        #endregion

        /// <summary>
        /// Calculates a new ratio for the <see cref="strokeRatio"/> based off the target <see cref="StrokeWidth"/> and the current <see cref="limitingSpan"/>.
        /// </summary>
        private void UpdateStrokeRatio()
        {
            strokeRatio = 1.0f - ((limitingSpan.SpanWidthInPixels - StrokeWidth) / limitingSpan.SpanWidthInPixels);
            isStrokeRatioDirty = false;
        }

        /// <summary>
        /// Invalidates the <see cref="canvas"/> of the given instance only if the animating is not running. Otherwise invalidating is not needed
        /// because the animation will apply the changes next cycle.
        /// </summary>
        /// <param name="widget"></param>
        private static void TryUpdate(LoadingWidget widget)
        {
            if (!widget.Animating) // Don't update if we are animating because the next frame will be updated anyway
                widget.canvas.InvalidateSurface();
        }
    }
}