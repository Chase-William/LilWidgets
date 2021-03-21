using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LilWidgets.Widgets
{
    public class CircularWidget : Widget
    {
       
        public CircularWidget() { }

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {            
            canvas.DrawText("Testing 123 can you see me?", rect.MidX, rect.MidY, new SKPaint()
            {
                Color = SKColors.Red,
                TextSize = 25
            });
        }
    }
}
