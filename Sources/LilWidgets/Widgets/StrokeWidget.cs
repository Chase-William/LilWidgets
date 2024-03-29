﻿/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
*/

using SkiaSharp;

namespace LilWidgets.Widgets
{
    /// <summary>
    /// A <see cref="StrokeWidget"/> is a supporting class for widgets that equal height and width with a stroke around the perimeter.
    /// </summary>
    public abstract class StrokeWidget : EquilateralWidget
    {
        #region Constants
        /// <summary>
        /// The base sigma for the drop shadow used with the backing arc.
        /// </summary>
        protected const float BASE_SHADOW_SIGMA = 2.5f;
        /// <summary>
        /// Default stroke width used for the arcs.
        /// </summary>
        public const float DEFAULT_STROKE_WIDTH_PERCENTAGE = 0.15f;
        /// <summary>
        /// Default shadow color used with the arcs.
        /// </summary>
        public static readonly SKColor defaultShadowColor = SKColors.LightGray;
        #endregion Constants

        #region Properties With Backing Fields    
        /// <summary>
        /// Gets or sets the color of the primary arc.
        /// </summary>
        public SKColor ArcColor
        {
            get => ArcPaint.Color;
            set
            {
                SKColor color = ArcPaint.Color;
                if (Set(ref color, value))                
                    ArcPaint.Color = value;                
            }
        }
        private SKColor shadowColor = defaultShadowColor;
        /// <summary>
        /// Gets or sets the color of the primary arc's shadow.
        /// </summary>
        public SKColor ShadowColor
        {
            get => shadowColor;
            set => Set(ref shadowColor, value);
        }       
        private float strokeWidthPercentage = DEFAULT_STROKE_WIDTH_PERCENTAGE;
        /// <summary>
        /// Gets or sets the width of the primary arc.
        /// </summary>
        public float StrokeWidthPercentage
        {
            get => strokeWidthPercentage;
            set
            {
                if (Set(ref strokeWidthPercentage, value))
                {                                  
                    UpdateFittedRect();
                    if (!IsAnimating)
                        OnInvalidateCanvas();
                }
            }
        }

        private SKRect fittedRect;
        /// <summary>
        /// Gets the rectangle for drawing that proper offsets applied for a stroke.
        /// </summary>
        protected SKRect FittedRect
        {
            get => fittedRect;
            set => Set(ref fittedRect, value);
        }

        private float offset;
        /// <summary>
        /// Gets the offset used when calculating the <see cref="FittedRect"/> from the <see cref="EquilateralWidget.EquilateralRect"/>.
        /// </summary>
        protected float Offset
        {
            get => offset;
            set => Set(ref offset, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="SKPaint"/> to draw the background arc.
        /// </summary>
        protected SKPaint ArcPaint { get; set; } = new SKPaint
        {
            Color = SKColors.White,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StrokeWidget"/> class.
        /// </summary>
        public StrokeWidget() { }

        protected override void OnCanvasRectChanged(in SKRectI rect)
        {
            base.OnCanvasRectChanged(in rect);
            UpdateFittedRect();
        }

        /// <summary>
        /// Updates the <see cref="FittedRect"/> property by calculating the correct offsets needed based off 
        /// the <see cref="StrokeWidthPercentage"/> while using the inherited <see cref="EquilateralWidget.EquilateralRect"/>.
        /// </summary>
        /// <param name="rect"></param>
        private void UpdateFittedRect()
        {
            float relativeStrokeWidth = LimitingDimensionLength * StrokeWidthPercentage / 2;
            // Compensate for the shadow
            float halfShadowStrokeWidth = relativeStrokeWidth / 2 + BASE_SHADOW_SIGMA;            

            // Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            if (LimitingDimension == Enumerations.LimitingDimensions.Height) // Canvas is wider than it is tall, hence compute for height
            {
                FittedRect = new SKRect
                {
                    Left = EquilateralRect.Left + halfShadowStrokeWidth,
                    Top = halfShadowStrokeWidth,
                    Right = EquilateralRect.Right - halfShadowStrokeWidth,
                    Bottom = EquilateralRect.Bottom - halfShadowStrokeWidth
                };
            }
            else // Width is the limiting dimension
            {
                FittedRect = new SKRect
                {
                    Left = halfShadowStrokeWidth,
                    Top = EquilateralRect.Top + halfShadowStrokeWidth,
                    Right = EquilateralRect.Right - halfShadowStrokeWidth,
                    Bottom = EquilateralRect.Bottom - halfShadowStrokeWidth
                };
            }

            ArcPaint.ImageFilter = SKImageFilter.CreateDropShadow(0,
                                                                  0,
                                                                  BASE_SHADOW_SIGMA,
                                                                  BASE_SHADOW_SIGMA,
                                                                  shadowColor);
            // Saving this offset to be used in other places like TextBounds calculation
            Offset = halfShadowStrokeWidth;

            // Update the paint StrokeWidth of our SKPaint
            ArcPaint.StrokeWidth = relativeStrokeWidth;
        }
    }
}
