/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using System;

using SkiaSharp;

namespace LilWidgets.Widgets
{
    public class LoadingWidget : StrokeWidget
    {
        public const float FULL_REVOLUTION = 360f;
        public const short MAX_ARC_LENGTH = 359;
        public const short MIN_ACC_LENGTH = 1;
        public const short DEFAULT_ARC_LENGTH = 90;
        public const short BASE_SWEEP_ANGLE = -90;
        private short arcLength = DEFAULT_ARC_LENGTH;
        public short ArcLength
        {
            get => arcLength;
            set 
            {
                if (value > MAX_ARC_LENGTH || value < MIN_ACC_LENGTH) // Outside Range Exception
                    throw new ArgumentOutOfRangeException($"Value {value} for property {nameof(ArcLength)} is outside the valid range of {MIN_ACC_LENGTH} to {MAX_ARC_LENGTH} inclusive.");
                if (Set(ref arcLength, value))
                    OnInvalidateCanvas();
            }
        }

        private float baseSweepAngle = BASE_SWEEP_ANGLE;
        public float BaseSweepAngle
        {
            get => baseSweepAngle;
            set
            {
                if (Set(ref baseSweepAngle, value))
                    OnInvalidateCanvas();
            }
        }

        public LoadingWidget()
            => AnimateCallback = (double value) => BaseSweepAngle = (float)value;        

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            // Creating paths
            var backgroundPath = new SKPath();
            backgroundPath.AddArc(FittedRect, BaseSweepAngle, ArcLength);
#if DEBUG
            System.Console.WriteLine($"BaseSweepAngle: {BaseSweepAngle}");
#endif
            
            // Draw Calls
            canvas.DrawPath(backgroundPath, ArcPaint); // Background Arc
        }
    }
}
