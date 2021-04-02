using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using SkiaSharp.Views.Android;

using LilWidgets.Widgets;
using LilWidgets.WeakEventHandlers;

namespace LilWidgets.Android.Views
{
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
                    Invalidate();

                    if (widget != null)
                    {                                                
                        handler = widget.ObserveChanges<InvalidatedWeakEventHandler<WidgetView>, WidgetView>(this, (view) => view.Invalidate());
                    }
                }
            }
        }
        public WidgetView(Context context) : base(context)
            => PaintSurface += OnPaintCanvas;        
        public WidgetView(Context context, IAttributeSet attributes) : base(context, attributes)
            => PaintSurface += OnPaintCanvas;        
        public WidgetView(Context context, IAttributeSet attributes, int defStyleAtt) : base(context, attributes, defStyleAtt)
            => PaintSurface += OnPaintCanvas;        
        public WidgetView(IntPtr ptr, JniHandleOwnership jni) : base(ptr, jni)
            => PaintSurface += OnPaintCanvas;        
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (widget != null)
            {
                var rect = e.Info.Rect;
                widget.Draw(e.Surface.Canvas, in rect);
            }
        }
    }
}