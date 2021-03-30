/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using System;

namespace LilWidgets.EventArguments
{
    public class IsAnimatingChangedEventArgs : EventArgs
    {
        public bool IsAnimating { get; set; }

        public IsAnimatingChangedEventArgs() { }

        public IsAnimatingChangedEventArgs(bool isAnimating)
            => IsAnimating = isAnimating;
    }
}
