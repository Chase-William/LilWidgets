using Xamarin.Forms;

using SkiaSharp.Views.Forms;
using SkiaSharp;

using LilWidgets.Forms.Extensions;
using LilWidgets.Widgets;

namespace LilWidgets.Forms.Views
{
    /// <summary>
    /// A <see cref="WidgetView"/> class that is the highest parent to all other derived widget-view classes.
    /// </summary>
    public abstract class WidgetView : SKCanvasView
    {
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="IsAnimating"/> property.
        /// </summary>
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(CircularWidgetView), false, BindingMode.OneWay, propertyChanged: OnAnimatingPropertyChanged);

        /// <summary>
        /// The underlying <see cref="Widget"/> that the <see cref="WidgetView"/> interfaces with.
        /// </summary>
        internal Widget UnderlyingWidget { get; set; }

        /// <summary>
        /// Indicates the state of the animation.
        /// True == Running, False == Inactive
        /// </summary>
        public bool IsAnimating
        {
            get => (bool)GetValue(IsAnimatingProperty);
            set => SetValue(IsAnimatingProperty, value);
        }

        private InvalidatedWeakEventHandler<WidgetView> handler;

        public WidgetView()
        {
            BackgroundColor = Color.Transparent;
            PaintSurface += OnPaintCanvas;
        }

        /// <summary>
        /// Triggers the underlying libraries draw methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (UnderlyingWidget != null)
            {
                var rect = e.Info.Rect;
                UnderlyingWidget.Draw(e.Surface.Canvas, in rect);
            }
            else
                e.Surface.Canvas.Clear(SKColors.Transparent);
        }

        private static void OnAnimatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<WidgetView>().GetCastedWidget<Widget>().IsAnimating = (bool)newValue;



        //private static void OnWidgetChanged(BindableObject bindable, object oldValue, object value)
        //{
        //    var view = bindable as WidgetView;

        //    if (view.UnderlyingWidget != null)
        //    {
        //        view.handler.Dispose();
        //        view.handler = null;
        //    }

        //    view.UnderlyingWidget = value as Widget;
        //    view.InvalidateSurface();

        //    if (view.UnderlyingWidget != null)
        //        view.handler = view.UnderlyingWidget.ObserveInvalidate(view, (v) => v.InvalidateSurface());
        //}
    }
}
