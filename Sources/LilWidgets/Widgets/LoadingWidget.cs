﻿/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
*/

using SkiaSharp;

using LilWidgets.Exceptions;

namespace LilWidgets.Widgets
{
    public class LoadingWidget : StrokeWidget
    {
        #region Constants
        // Todo: comment constants
        public const float FULL_REVOLUTION = 360f;
        public const short MAX_ARC_LENGTH = 359;
        public const short MIN_ACC_LENGTH = 1;
        public const short DEFAULT_ARC_LENGTH = 90;
        public const short BASE_SWEEP_ANGLE = -90;        
        #endregion

        #region Properties With Backing Fields
        private short arcLength = DEFAULT_ARC_LENGTH;
        /// <summary>
        /// Gets or sets the length of the Arc in degrees. 
        /// The max value is denoted by <see cref="MAX_ARC_LENGTH"/> and likewise the min value is denoted by <see cref="MIN_ACC_LENGTH"/>.
        /// </summary>
        public short ArcLength
        {
            get => arcLength;
            set 
            {
                if (value > MAX_ARC_LENGTH || value < MIN_ACC_LENGTH) // Range constraints
                    throw new PropertyValueOutOfRangeException(value, MIN_ACC_LENGTH, MAX_ARC_LENGTH);
                if (Set(ref arcLength, value))
                    OnInvalidateCanvas();
            }
        }

        private float baseSweepAngle = BASE_SWEEP_ANGLE;
        /// <summary>
        /// Gets or sets the starting position of the arc in degrees.
        /// </summary>
        public float BaseSweepAngle
        {
            get => baseSweepAngle;
            set
            {
                if (Set(ref baseSweepAngle, value))
                    OnInvalidateCanvas();
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new <see cref="LoadingWidget"/> instance.
        /// </summary>
        public LoadingWidget()
            => AnimateCallback = (double value) => BaseSweepAngle = (float)value;        

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            // Creating paths
            var backgroundPath = new SKPath();
            backgroundPath.AddArc(FittedRect, BaseSweepAngle, ArcLength);
#if DEBUG
            //System.Console.WriteLine($"BaseSweepAngle: {BaseSweepAngle}");
#endif
            
            // Draw Calls
            canvas.DrawPath(backgroundPath, ArcPaint); // Background Arc
        }
    }
}
