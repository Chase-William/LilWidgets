/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using SkiaSharp;

namespace LilWidgets.Widgets
{
    public class LoadingWidget : StrokeWidget
    {
        /// <summary>
        /// The starting position of the arcs.
        /// </summary>
        const float SWEEP_START = -90;

        public const float FULL_REVOLUTION = 360f;
        public const short MAX_ARC_LENGTH = 359;
        const string ANIMATION_IDENTIFER = "loading";

        public const short DEFAULT_ARC_LENGTH = 90;

        private short arcLength = DEFAULT_ARC_LENGTH;
        public short ArcLength
        {
            get => arcLength;
            set 
            {
                if (Set(ref arcLength, value))
                    Invalidate();
            }
        }

        private float baseSweepAngle = 0;
        public float BaseSweepAngle
        {
            get => baseSweepAngle;
            set
            {
                if (Set(ref baseSweepAngle, value))
                    Invalidate();
            }
        }

        public LoadingWidget()
        {
            AnimateCallback = (double value) =>
            {
                BaseSweepAngle = (float)value;
            };

            //AnimateCallback = new Animation((value) =>
            //{
            //    baseSweepAngle = (float)value;
            //    canvas.InvalidateSurface();
            //}, baseSweepAngle, baseSweepAngle + FULL_REVOLUTION);
            //animation.Commit(this,
            //                ANIMATION_IDENTIFER,
            //                16,
            //                Duration,
            //                null,
            //                null,
            //                () => IsAnimating);
        }

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            // Creating paths
            var backgroundPath = new SKPath();
            backgroundPath.AddArc(FittedRect, BaseSweepAngle, ArcLength);

            System.Console.WriteLine($"BaseSweepAngle: {BaseSweepAngle}");

            
            // Draw Calls
            canvas.DrawPath(backgroundPath, ArcPaint); // Background Arc                             
        }
    }
}
