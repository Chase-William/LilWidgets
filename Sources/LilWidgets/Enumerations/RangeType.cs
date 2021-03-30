/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

namespace LilWidgets.Enumerations
{
    /// <summary>
    /// Describes whether a range's absolute max or min is inclusive or exclusive.
    /// </summary>
    public enum RangeType
    {
        /// <summary>
        /// Mathematically expressed in interval notation with parenthesis.
        /// </summary>
        Inclusive,
        /// <summary>
        /// Mathematically expressed in interval notation with brackets.
        /// </summary>
        Exclusive
    }
}
