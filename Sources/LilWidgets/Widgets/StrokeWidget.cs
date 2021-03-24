/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
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
        protected const float BASE_SHADOW_SIGMA = 3f;        
        /// <summary>
        /// The default value for the duration of the progress animation.
        /// </summary>
        protected const uint DEFAULT_ANIMATION_DURATION = 1000;
        /// <summary>
        /// The default stroke width used for the arcs.
        /// </summary>
        protected const float DEFAULT_STROKE_WIDTH = 10;

        protected const float DEFAULT_STOKE_RATIO = 0.1f;
        #endregion Constants

        #region Properties
        private SKColor strokeColor;
        /// <summary>
        /// Gets or sets the color of the primary arc.
        /// </summary>
        public SKColor ArcColor
        {
            get => strokeColor;
            set => Set(ref strokeColor, value);
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
            set
            {
                if (Set(ref strokeWidth, value, notifyPropertyChanged: false))
                {
                    // Disable propChanged so we can re-calculate StrokeRatio before the next frame
                    IsStrokeRatioDirty = true;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        protected float StrokeRatio { get; private set; } = DEFAULT_STOKE_RATIO;

        protected bool IsStrokeRatioDirty { get; private set; }

        protected SKRect FittedRect { get; private set; }

        /// <summary>
        /// Used to draw the background arc.
        /// </summary>
        protected SKPaint ArcPaint { get; set; } = new SKPaint
        {
            Color = SKColors.White,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, BASE_SHADOW_SIGMA, BASE_SHADOW_SIGMA, SKColors.DarkGray)
        };

        protected SKRectI StrokedRect { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrokeWidget"/> class.
        /// </summary>
        public StrokeWidget() { }

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            if (IsStrokeRatioDirty) // Calculate the correct strokeRatio if needed
                UpdateStrokeRatio(rect);
            
            //float relativeStrokeWidth = LimitingDimensionLength * StrokeRatio;
            //float halfOfRelativeStrokeWidth = relativeStrokeWidth / 2;
            //float relativeShadowSigma = BASE_SHADOW_SIGMA + BASE_SHADOW_SIGMA * StrokeRatio;
            //// Compensate for the shadow
            //float halfShadowStrokeWidth = halfOfRelativeStrokeWidth + relativeShadowSigma * 3f;
            
            //// Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            //if (LimitingDimension == Enumerations.LimitingDimensions.Height) // Canvas is wider than it is tall, hence compute for height
            //{
            //    FittedRect = new SKRect(rect.MidX - rect.MidY + halfShadowStrokeWidth, // left
            //                         halfShadowStrokeWidth, // top
            //                         rect.MidX + rect.MidY - halfShadowStrokeWidth, // right
            //                         rect.Height - halfShadowStrokeWidth); // bottom
            //}
            //else // Canvas is taller than it is wide so compute for width
            //{
            //    FittedRect = new SKRect(halfShadowStrokeWidth, // left
            //                         rect.MidY - rect.MidX + halfShadowStrokeWidth, // top
            //                         rect.Width - halfShadowStrokeWidth, // right
            //                         rect.MidY + rect.MidX - halfShadowStrokeWidth); // bottom
            //}

            //if (FittedRect.Width < 0 || FittedRect.Height < 0) // return if the control is becoming negatively sized
            //    return;
            //if (relativeStrokeWidth > FittedRect.Width)
            //    return;

            //ArcPaint.ImageFilter = SKImageFilter.CreateDropShadow(0,
            //                                                      0,
            //                                                      relativeShadowSigma,
            //                                                      relativeShadowSigma,
            //                                                      shadowColor);

            // Applying path widths aka strike widths
            //ArcPaint.StrokeWidth = relativeStrokeWidth;
        }

        private void UpdateStrokeRatio(in SKRectI rect)
        {
            StrokeRatio = 1.0f - (LimitingDimensionLength - StrokeWidth) / LimitingDimensionLength;
            IsStrokeRatioDirty = false;
        }

        protected override void OnCanvasRectChanged(in SKRectI rect)
        {
            base.OnCanvasRectChanged(in rect);
            float relativeStrokeWidth = LimitingDimensionLength * StrokeRatio;
            float halfOfRelativeStrokeWidth = relativeStrokeWidth / 2;
            float relativeShadowSigma = BASE_SHADOW_SIGMA + BASE_SHADOW_SIGMA * StrokeRatio;
            // Compensate for the shadow
            float halfShadowStrokeWidth = halfOfRelativeStrokeWidth + relativeShadowSigma * 3f;

            // Determine top / bottom by finding the MidY then subtracting half the target width (get the radius of our circle) then subtract the half of stroke which acts as an offset
            if (LimitingDimension == Enumerations.LimitingDimensions.Height) // Canvas is wider than it is tall, hence compute for height
            {
                FittedRect = new SKRect(rect.MidX - rect.MidY + halfShadowStrokeWidth, // left
                                     halfShadowStrokeWidth, // top
                                     rect.MidX + rect.MidY - halfShadowStrokeWidth, // right
                                     rect.Height - halfShadowStrokeWidth); // bottom
            }
            else // Canvas is taller than it is wide so compute for width
            {
                FittedRect = new SKRect(halfShadowStrokeWidth, // left
                                     rect.MidY - rect.MidX + halfShadowStrokeWidth, // top
                                     rect.Width - halfShadowStrokeWidth, // right
                                     rect.MidY + rect.MidX - halfShadowStrokeWidth); // bottom
            }

            if (FittedRect.Width < 0 || FittedRect.Height < 0) // return if the control is becoming negatively sized
                return;
            if (relativeStrokeWidth > FittedRect.Width)
                return;

            ArcPaint.ImageFilter = SKImageFilter.CreateDropShadow(0,
                                                                  0,
                                                                  relativeShadowSigma,
                                                                  relativeShadowSigma,
                                                                  shadowColor);

            ArcPaint.StrokeWidth = relativeStrokeWidth;
        }
    }
}
