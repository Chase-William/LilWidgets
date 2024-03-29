﻿/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
*/

using System;

using SkiaSharp;

using LilWidgets.Exceptions;
using System.Runtime.CompilerServices;

namespace LilWidgets.Widgets
{
    public class ProgressWidget : StrokeWidget
    {
        #region Constants
        // Todo: comment constants
        public const float DEFAULT_PROGRESS_PERCENTAGE = 0;
        public const short BASE_SWEEP_ANGLE = -90;
        public const bool DEFAULT_IS_TEXT_VISIBLE = true;
        public const float MIN_PERCENTAGE = 0f;
        public const float MAX_PERCENTAGE = 1f;
        public const bool DEFAULT_AUTO_ANIMATE = false;
        public const float DEFAULT_TEXT_SIZE_PERCENTAGE = 0.8f;
        public const float MIN_TEXT_SIZE_PERCENTAGE = 0.01f;
        #endregion

        #region Properties With Backing Fields
        private float progressPercentage;
        /// <summary>
        /// Gets or sets the target percentage to be displayed by the <see cref="ProgressWidget"/>.
        /// </summary>
        public float ProgressPercentage
        {
            get => progressPercentage;
            set
            {
                if (value > MAX_PERCENTAGE || value < MIN_PERCENTAGE) // Throw exception if invalid percentage given.
                    throw new PropertyValueOutOfRangeException(value, MIN_PERCENTAGE, MAX_PERCENTAGE);
                if (Set(ref progressPercentage, value))
                {
                    if (AutoAnimate) // Restart the running animation to use updated values
                        RestartAnimation();                    
                }
            }
        }

        private float currentProgressPercentage;
        /// <summary>
        /// Gets the current percentage the <see cref="ProgressWidget"/> is displaying.
        /// This value will be updating constantly while the animation is running.
        /// </summary>
        public float CurrentProgressPercentage
        {
            get => currentProgressPercentage;
            protected set
            {
                currentProgressPercentage = value;
                if (currentProgressPercentage == ProgressPercentage)
                    IsAnimating = false; // Mark the animation as off when the actual is equal to the target
                OnInvalidateCanvas();
            }
        }

        private bool isTextVisible = DEFAULT_IS_TEXT_VISIBLE;
        /// <summary>
        /// Gets or sets whether the percentage text should be visible.
        /// </summary>
        public bool IsTextVisible
        {
            get => isTextVisible;
            set
            {
                if (Set(ref isTextVisible, value) && !IsAnimating)                
                    OnInvalidateCanvas(); // Update the canvas when a new value is given and we are not currently animating.                
            }
        }

        private float textSizePercentage = DEFAULT_TEXT_SIZE_PERCENTAGE;
        /// <summary>
        /// Gets or sets the size of the text within the <see cref="StrokeWidget.FittedRect"/>.
        /// </summary>
        public float TextSizePercentage
        {
            get => textSizePercentage;
            set
            {
                if (value > MAX_PERCENTAGE || value < MIN_TEXT_SIZE_PERCENTAGE) // Throw exception if invalid percentage given.
                    throw new PropertyValueOutOfRangeException(value, MIN_TEXT_SIZE_PERCENTAGE, MAX_PERCENTAGE);
                if (Set(ref textSizePercentage, value) && !IsAnimating)                
                    OnInvalidateCanvas(); // Update the canvas when a new value is given and we are not currently animating.         
            }
        }      

        private bool autoAnimate = DEFAULT_AUTO_ANIMATE;
        /// <summary>
        /// Gets or sets whether assigning a new value to the <see cref="ProgressPercentage"/> automatically starts the animation.
        /// </summary>
        public bool AutoAnimate
        {
            get => autoAnimate;
            set => Set(ref autoAnimate, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="SKPaint"/> being used for drawing the text displaying the <see cref="CurrentProgressPercentage"/> property.
        /// </summary>
        protected SKPaint TextPaint { get; set; } = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            TextSize = 1
        };
        #endregion

        #region Standalone Fields
        /// <summary>
        /// Max width available inside the offsets of <see cref="StrokeWidget.FittedRect"/>.
        /// This is used when embedding for say text inside a rectangle.
        /// </summary>
        private float maxWidthInsideFittedRect = 0;
        #endregion

        /// <summary>
        /// Initializes a new <see cref="ProgressWidget"/> instance.
        /// </summary>
        public ProgressWidget()
            => AnimateCallback = (double value) => CurrentProgressPercentage = (float)value;

        /// <summary>
        /// Gets the relative duration remaining to finish this animation.
        /// </summary>
        /// <returns></returns>
        public uint GetRelativeDuration()
            => (uint)(Duration * Math.Abs(ProgressPercentage - CurrentProgressPercentage));        

        protected override void OnInvalidateAnimation()
        {
            if (CurrentProgressPercentage == ProgressPercentage)
                return; // Return if there is nothing to animate

            base.OnInvalidateAnimation();
        }       

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            // Creating paths
            var progressPath = new SKPath();
            progressPath.AddArc(FittedRect, BASE_SWEEP_ANGLE, CurrentProgressPercentage * 360);

#if DEBUG
            System.Console.WriteLine("CurrentProgressPercentage: " + CurrentProgressPercentage);
#endif
            // Draw Calls
            canvas.DrawPath(progressPath, ArcPaint); // Progress Arc

            if (IsTextVisible) // Draw text only if enabled
            {
                string percentageMsg = CurrentProgressPercentage.ToString("P");                
                // Adjust TextSize property so text is 90% of screen width
                float textWidth = TextPaint.MeasureText(percentageMsg);
                TextPaint.TextSize = TextSizePercentage * maxWidthInsideFittedRect * TextPaint.TextSize / textWidth;
                // Getting the text's bounds
                SKRect textBounds = new SKRect();
                // Find the text bounds
                TextPaint.MeasureText(percentageMsg, ref textBounds);
                // Draw text in the center of the control vertically and horizontally
                canvas.DrawText(percentageMsg,
                                FittedRect.MidX - textBounds.MidX,
                                FittedRect.MidY - textBounds.MidY,
                                TextPaint); // Progress Text
            }
        }

        protected override void OnNotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            base.OnNotifyPropertyChanged(prop);
            if (prop == nameof(Offset)) // When Offset changes we need to update this variable as well for our text.
                maxWidthInsideFittedRect = FittedRect.Width - Offset * 2;
        }
    }
}
