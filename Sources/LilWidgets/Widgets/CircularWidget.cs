using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LilWidgets.Widgets
{
    /// <summary>
    /// A <see cref="CircularWidget"/> is a supporting class for circular-widgets.
    /// </summary>
    public abstract class CircularWidget : Widget
    {
        #region Properties
        private SKColor arcColor;
        /// <summary>
        /// Gets or sets the color of the primary arc.
        /// </summary>
        public SKColor ArcColor
        {
            get => arcColor;
            set => Set(ref arcColor, value);
        }
        private SKColor shadowColor;
        /// <summary>
        /// Gets or sets the color of the primary arc's shadow.
        /// </summary>
        public SKColor ShadowColor
        {
            get => shadowColor;
            set => Set(ref shadowColor, value);
        }
        private uint duration;
        /// <summary>
        /// Gets or sets the time in milliseconds for a one complete cycle of the animation.
        /// </summary>
        public uint Duration
        {
            get => duration;
            set => Set(ref duration, value);
        }
        private float strokeWidth;
        /// <summary>
        /// Gets or sets the width of the primary arc.
        /// </summary>
        public float StrokeWidth
        {
            get => strokeWidth;
            set => Set(ref strokeWidth, value);
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularWidget"/> class.
        /// </summary>
        public CircularWidget() { }


        //public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        //{

        //}



        //public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        //{
        //    base.DrawContent(canvas, in rect);
        //    //canvas.DrawText("Testing 123 can you see me?", rect.MidX, rect.MidY, new SKPaint()
        //    //{
        //    //    Color = SKColors.Red,
        //    //    TextSize = 25
        //    //});
        //}
    }
}
