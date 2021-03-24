/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using SkiaSharp;

using LilWidgets.Enumerations;
using LilWidgets.Structures;

namespace LilWidgets.Widgets
{
    /// <summary>
    /// A <see cref="EquilateralWidget"/> is a supporting class for a widget that needs a square rectangle for its content to be drawn in.
    /// This rectangle will be centered inside the parent rectangle automatically.
    /// </summary>
    public abstract class EquilateralWidget : Widget
    {
        /// <summary>
        /// Indicates whether the width and height are equal in length or which is the smaller of the two.
        /// </summary>
        public LimitingDimensions LimitingDimension { get; private set; }

        /// <summary>
        /// Length of the limiting span given in the <see cref="Update(float, float)"/> method parameters.
        /// </summary>
        public float LimitingDimensionLength { get; private set; }                

        protected override void OnCanvasRectChanged(in SKRectI rect)
        {
            if (rect.Height > rect.Width) // The width is the smaller span of the two
            {
                LimitingDimension = LimitingDimensions.Width;
                LimitingDimensionLength = rect.Width;
            }
            else if (rect.Height < rect.Width) // The height is the smaller span of the two
            {

                LimitingDimension = LimitingDimensions.Height;
                LimitingDimensionLength = rect.Height;
            }
            else // The width and height are equal in value
            {
                LimitingDimension = LimitingDimensions.Equal;
                LimitingDimensionLength = rect.Width;
            }
        }
    }
}
