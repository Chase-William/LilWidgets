/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using SkiaSharp;

namespace LilWidgets.Widgets
{
    public class ProgressWidget : StrokeWidget
    {
        public const float DEFAULT_PROGRESS_PERCENTAGE = 0;
        public const short BASE_SWEEP_ANGLE = -90;
        public const bool DEFAULT_IS_TEXT_VISIBLE = true;

        private float progressPercentage;
        public float ProgressPercentage
        {
            get => progressPercentage;
            set
            {
                if (Set(ref progressPercentage, value))
                {
                    if (AutoAnimate) // Restart the running animation to use updated values
                        RestartAnimation();                    
                }
            }
        }

        private float currentProgressPercentage;
        public float CurrentProgressPercentage
        {
            get => currentProgressPercentage;
            protected set
            {
                currentProgressPercentage = value;
                OnInvalidateCanvas();
            }
        }

        public bool IsTextVisible { get; set; } = DEFAULT_IS_TEXT_VISIBLE;

        public bool AutoAnimate { get; set; }

        private SKPaint textPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

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
            => (uint)(Duration * (ProgressPercentage - CurrentProgressPercentage));

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


            //var backgroundPath = new SKPath();
            //backgroundPath.AddArc(arcRect, SWEEP_START, 360f);
            // Applying path widths aka strike widths
            //backgroundPaint.StrokeWidth = relativeStrokeWidth;
            // Draw Calls
            //canvas.DrawPath(backgroundPath, backgroundPaint); // Background Arc
            canvas.DrawPath(progressPath, ArcPaint); // Progress Arc

            if (IsTextVisible) // Draw text only if enabled
            {
                string percentageMsg = CurrentProgressPercentage.ToString("P");
                // Adjust TextSize property so text is 75% of screen width
                var textWidth = textPaint.MeasureText(percentageMsg);
                float width = FittedRect.Width / 2;
                if (width > 1) // We don't want *lose* the text, the IsTextVisible property should be used for hiding the text
                {
                    textPaint.TextSize = width * textPaint.TextSize / textWidth;
                }
                SKRect textBounds = new SKRect();
                // Find the text bounds
                textPaint.MeasureText(percentageMsg, ref textBounds);
                // Draw text in the center of the control vertically and horizontally
                canvas.DrawText(percentageMsg,
                                FittedRect.MidX - textBounds.MidX,
                                FittedRect.MidY - textBounds.MidY,
                                textPaint); // Progress Text
            }
        }
    }
}
