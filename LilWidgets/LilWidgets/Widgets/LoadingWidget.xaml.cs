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

        const float FULL_REVOLUTION = 360f;
        const short MAX_ARC_LENGTH = 359;
        const string ANIMATION_IDENTIFER = "loading";

        const short DEFAULT_ARC_LENGTH = 90;
        /// <summary>
        /// The default value for the duration of the progress animation.
        /// </summary>
        public const uint DEFAULT_ANIMATION_DURATION = 1000;
        /// <summary>
        /// The default stroke width used for the arcs.
        /// </summary>
        public const float DEFAULT_STROKE_WIDTH = 10;
        /// <summary>
        /// The default shadow color used with the arcs.
        /// </summary>
        public static readonly Color defaultShadowColor = Color.FromHex("#5555");
        #endregion Constants

        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcColorProperty"/> property.
        /// </summary>
        public static readonly BindableProperty ArcColorProperty = BindableProperty.Create(nameof(ArcColor), typeof(Color), typeof(LoadingWidget), Color.Black, BindingMode.OneWay, null, ArcColorPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="Duration"/> property.
        /// </summary>
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(LoadingWidget), DEFAULT_ANIMATION_DURATION, BindingMode.OneWay);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(LoadingWidget), DEFAULT_STROKE_WIDTH, BindingMode.OneWay, null, StokeWidthPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ShadowColor"/> property.
        /// </summary>
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(LoadingWidget), defaultShadowColor, BindingMode.OneWay, null, ShadowColorPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="IsAnimating"/> property.
        /// </summary>
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(LoadingWidget), false, BindingMode.OneWay, null, AnimatingPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcLength"/> property.
        /// </summary>
        public static readonly BindableProperty ArcLengthProperty = BindableProperty.Create(nameof(ArcLength), typeof(short), typeof(LoadingWidget), DEFAULT_ARC_LENGTH, BindingMode.OneWay, ValidateValueDelegate, ArcLengthPropertyChanged);
        #endregion Bind-able Properties

        #region Properties   
        /// <summary>
        /// The length of the arc in degrees relative to it's start position. 
        /// For example the value of <see cref="DEFAULT_ARC_LENGTH"/> is 90 and that would be a quarter of a full 360 degree arc.
        /// </summary>
        public short ArcLength
        {
            get => (short)GetValue(ArcLengthProperty);
            set => SetValue(ArcLengthProperty, value);
        }
        /// <summary>
        /// The color of the arc.
        /// </summary>
        public Color ArcColor
        {
            get => (Color)GetValue(ArcColorProperty);
            set => SetValue(ArcColorProperty, value);
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
        public uint Duration
        {
            get => (uint)GetValue(DurationProperty);
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
        /// <summary>
        /// Determines the state of the animation.
        /// True == Running, False == Inactive
        /// </summary>
        public bool IsAnimating
        {
            get => (bool)GetValue(IsAnimatingProperty);
            set => SetValue(IsAnimatingProperty, value);
        }
        #endregion Properties

        #region Fields
        /// <summary>
        /// Used to draw the background arc.
        /// </summary>
        SKPaint arcPaint = new SKPaint
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
        /// The mid-x value of the <see cref="canvas"/>. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float midX = 0;
        /// <summary>
        /// The mid-y value of the <see cref="canvas"/>. Updates come from the <see cref="canvas_PaintSurface(object, SKPaintSurfaceEventArgs)"/> method.
        /// </summary>
        float midY = 0;
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
        /// The base sweep angle that determines the loading arcs starting angle to offset from.
        /// </summary>
        float baseSweepAngle = SWEEP_START;
        /// <summary>
        /// The animation's supporting Xamarin.Forms type. Carries out the actual animating and manipulation of the animation values.
        /// </summary>
        Animation animation;
        /// <summary>
        /// Contains the width and height of this composite view. Provides extra information about the relationship between the two "spans"
        /// that are used in the rendering pipeline.
        /// </summary>
        private LimitingSpan limitingSpan = new LimitingSpan(Util.DisplayUtil.DPI);

        #endregion Fields
        

        /// <summary>
        /// Primary Constructor.
        /// </summary>
        public LoadingWidget() => InitializeComponent();

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            limitingSpan.Update((float)width, (float)height);
        }       

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

            //limitingSpan = (isWidthGreaterThanHeight ? info.Height : info.Width);

            if (isStrokeRatioDirty) // Calculate the correct strokeRatio if needed
                UpdateStrokeRatio();

            relativeStrokeWidth = limitingSpan.SpanLengthInPixels * strokeRatio;
            halfOfRelativeStrokeWidth = relativeStrokeWidth / 2;
            relativeShadowSigma = BASE_SHADOW_SIGMA + BASE_SHADOW_SIGMA * strokeRatio;
            // Compensate for the shadow
            halfShadowStrokeWidth = halfOfRelativeStrokeWidth + relativeShadowSigma * 3f;

            // Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            if (limitingSpan.IsHeightTheLimitingSpan) // Canvas is wider than it is tall, hence computer for height
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
            Debug.WriteLine($"Degrees: {baseSweepAngle}");
#endif
            // Set the shadow for the background arc
            arcPaint.ImageFilter = SKImageFilter.CreateDropShadow(0,
                                                                  0,
                                                                  relativeShadowSigma,
                                                                  relativeShadowSigma,
                                                                  shadowColor);
           
            // Creating paths
            backgroundPath = new SKPath();
            backgroundPath.AddArc(arcRect, baseSweepAngle, ArcLength);
            
            // Applying path widths aka strike widths
            arcPaint.StrokeWidth = relativeStrokeWidth;
            // Draw Calls
            canvas.DrawPath(backgroundPath, arcPaint); // Background Arc
        }        

        /// <summary>
        /// Begins the animation process by committing the animation to execute.
        /// </summary>
        private void BeginAnimation()
        {
            animation = new Animation((value) =>
            {
                baseSweepAngle = (float)value;
                canvas.InvalidateSurface();
            }, baseSweepAngle, baseSweepAngle + FULL_REVOLUTION);
            animation.Commit(this,
                            ANIMATION_IDENTIFER,
                            16,
                            Duration,
                            null,
                            null,
                            () => IsAnimating);          
        }

        #region Validation Property Changed Handlers
        private static bool ValidateValueDelegate(BindableObject bindable, object value)
        {
            short lengthDegrees = (short)value;
            if (lengthDegrees < 0 || lengthDegrees > MAX_ARC_LENGTH)
                return false;
            return true;
        }
        #endregion

        #region Property Change Handlers
        private static void ArcLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => TryUpdate((LoadingWidget)bindable);

        private static void ArcColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            LoadingWidget widget = (LoadingWidget)bindable;
            widget.arcPaint.Color = ((Color)newValue).ToSKColor();
            TryUpdate(widget);
        }
        /// <summary>
        /// Converts the bind-able property's value and assigns it to the <see cref="isStrokeRatioDirty"/> property.
        /// </summary>
        /// <param name="bindable">Declaring type.</param>
        /// <param name="oldValue">Old stroke width value used by both backing and front rings.</param>
        /// <param name="newValue">New stroke width value used by both backing and front rings.</param>
        private static void StokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (LoadingWidget)bindable;
            widget.isStrokeRatioDirty = true;
            TryUpdate(widget);
        }
        /// <summary>
        /// Coverts the bind-able property's value and assigns it to the <see cref="shadowColor"/>'s property.
        /// </summary>
        /// <param name="bindable">Declaring type.</param>
        /// <param name="oldValue">Old shadow color value used only by the backing ring.</param>
        /// <param name="newValue">New shadow color value used only by the backing ring.</param>
        private static void ShadowColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (LoadingWidget)bindable;
            widget.shadowColor = ((Color)newValue).ToSKColor();
            TryUpdate(widget);
        }
        /// <summary>
        /// Informs the backing <see cref="animation"/> field of the change. If the value is true a new animation is created and starts off
        /// where the previous left off. If the value is false the current animation is aborted immediately.
        /// </summary>
        /// <param name="bindable">Declaring type.</param>
        /// <param name="oldValue">The old value that indicates if the animation was active or inactive.</param>
        /// <param name="newValue">The new value.</param>
        private static void AnimatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var widget = (LoadingWidget)bindable;
            if ((bool)newValue) // Begin animation
                widget.BeginAnimation();
            else // Cancel animation
                widget.AbortAnimation(ANIMATION_IDENTIFER);
        }
        #endregion

        /// <summary>
        /// Calculates a new ratio for the <see cref="strokeRatio"/> based off the target <see cref="StrokeWidth"/> and the current <see cref="limitingSpan"/>.
        /// </summary>
        private void UpdateStrokeRatio()
        {
            strokeRatio = 1.0f - ((limitingSpan.SpanLengthInPixels - StrokeWidth) / limitingSpan.SpanLengthInPixels);
            isStrokeRatioDirty = false;
        }

        /// <summary>
        /// Invalidates the <see cref="canvas"/> of the given instance only if the animating is not running. Otherwise invalidating is not needed
        /// because the animation will apply the changes next cycle.
        /// </summary>
        /// <param name="widget"></param>
        private static void TryUpdate(LoadingWidget widget)
        {
            if (!widget.IsAnimating) // Don't update if we are animating because the next frame will be updated anyway
                widget.canvas.InvalidateSurface();
        }
    }
}