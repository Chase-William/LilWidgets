using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using LilWidgets.Widgets;
using SkiaSharp;

namespace LilWidgets.Forms.Views
{
    public abstract class WidgetView : SKCanvasView
    {
        public static readonly BindableProperty WidgetProperty = BindableProperty.Create(nameof(Widget), typeof(Widget), typeof(WidgetView), propertyChanged: OnWidgetChanged);
        private InvalidatedWeakEventHandler<WidgetView> handler;
        private Widget widget;
        public Widget Widget
        {
            get => (Widget)GetValue(WidgetProperty);
            set => SetValue(WidgetProperty, value);
        }
        public WidgetView()
        {
            BackgroundColor = Color.Transparent;
            PaintSurface += OnPaintCanvas;
        }
        private static void OnWidgetChanged(BindableObject bindable, object oldValue, object value)
        {
            var view = bindable as WidgetView;

            if (view.widget != null)
            {
                view.handler.Dispose();
                view.handler = null;
            }

            view.widget = value as Widget;
            view.InvalidateSurface();

            if (view.widget != null)            
                view.handler = view.widget.ObserveInvalidate(view, (v) => v.InvalidateSurface());            
        }

        /// <summary>
        /// Triggers the underlying libraries draw methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (widget != null)
            {
                var rect = e.Info.Rect;
                widget.Draw(e.Surface.Canvas, in rect);
            }          
            else            
                e.Surface.Canvas.Clear(SKColors.Transparent);            
        }
    }
}
