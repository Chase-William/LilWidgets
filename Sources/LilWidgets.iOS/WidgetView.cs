using Foundation;
using UIKit;

using System;

using SkiaSharp;
using SkiaSharp.Views.iOS;

using LilWidgets.Widgets;
using LilWidgets.WeakEventHandlers;

namespace LilWidgets.iOS
{
    [Register("WidgetView")]
    public class WidgetView : SKCanvasView
    {
        private InvalidatedWeakEventHandler<WidgetView> handler;
        private Widget widget;
        public Widget Widget
        {
            get => widget;
            set
            {
                if (widget != value)
                {
                    if (widget != null)
                    {
                        handler.Dispose();
                        handler = null;
                    }

                    widget = value;
                    InvalidateWidget();

                    if (widget != null)
                    {
                        handler = widget.ObserveChanges<InvalidatedWeakEventHandler<WidgetView>, WidgetView>(this, (view) => view.InvalidateWidget());
                    }
                }
            }
        }        
        public WidgetView()
            => Initialize();
        public WidgetView(IntPtr handle) : base(handle) { }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }
        private void Initialize()
        {
            BackgroundColor = UIColor.Clear;
            PaintSurface += OnPaintCanvas;
        }
        private void InvalidateWidget() => SetNeedsDisplayInRect(Bounds);
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