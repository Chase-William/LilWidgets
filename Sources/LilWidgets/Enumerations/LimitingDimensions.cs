/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
*/

namespace LilWidgets.Enumerations
{
    /// <summary>
    /// Limiting dimension of a rectangle.
    /// </summary>
    public enum LimitingDimensions
    {
        /// <summary>
        /// Dimensions are equal.
        /// </summary>
        Equal,
        /// <summary>
        /// Smaller than <see cref="Height"/>.
        /// </summary>
        Width,
        /// <summary>
        /// Smaller than <see cref="Width"/>.
        /// </summary>
        Height
    }
}
