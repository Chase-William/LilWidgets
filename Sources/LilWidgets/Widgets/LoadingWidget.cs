using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Widgets
{
    public class LoadingWidget : CircularWidget
    {
        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            //base.DrawContent(canvas, in rect);
            canvas.DrawText("Hello World!", rect.MidX, rect.MidY, new SKPaint
            {
                Color = SKColors.Red,
                TextSize = 25
            });            
        }
    }
}
