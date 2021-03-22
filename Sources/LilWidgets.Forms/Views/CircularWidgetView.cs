using Xamarin.Forms;

using SkiaSharp.Views.Forms;

using LilWidgets.Widgets;
using LilWidgets.Forms.Extensions;

namespace LilWidgets.Forms.Views
{
    /// <summary>
    /// A <see cref="CircularWidgetView"/> is a supporting class for circular-widget-views.
    /// </summary>
    public abstract class CircularWidgetView : WidgetView
    {
        #region Constants
        /// <summary>
        /// The default value for the duration of the progress animation.
        /// </summary>
        public const uint DEFAULT_ANIMATION_DURATION = 1000;
        /// <summary>
        /// The default stroke width used for the arcs.
        /// </summary>
        public const float DEFAULT_STROKE_WIDTH = 15;
        /// <summary>
        /// The default shadow color used with the arcs.
        /// </summary>
        public static readonly Color defaultShadowColor = Color.FromHex("#5555");
        #endregion Constants

        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcColorProperty"/> property.
        /// </summary
        public static readonly BindableProperty ArcColorProperty = BindableProperty.Create(nameof(ArcColor), typeof(Color), typeof(CircularWidgetView), Color.Black, BindingMode.OneWay, propertyChanged: OnArcColorPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="Duration"/> property.
        /// </summary>
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(CircularWidgetView), DEFAULT_ANIMATION_DURATION, BindingMode.OneWay, propertyChanged: OnDurationPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(CircularWidgetView), DEFAULT_STROKE_WIDTH, BindingMode.OneWay, propertyChanged: OnStrokeWidthPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ShadowColor"/> property.
        /// </summary>
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(CircularWidgetView), defaultShadowColor, BindingMode.OneWay, propertyChanged: OnShadowColorPropertyChanged);               
        #endregion Bind-able Properties

        #region Properties           
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
        #endregion Properties

        #region OnPropertyChanged Handlers
        private static void OnArcColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<CircularWidget>().ArcColor = ((Color)newValue).ToSKColor();
        private static void OnShadowColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<CircularWidget>().ShadowColor = ((Color)newValue).ToSKColor();
        private static void OnDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<CircularWidget>().Duration = (uint)newValue;
        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<CircularWidget>().StrokeWidth = (float)newValue;
        #endregion
    }
}
