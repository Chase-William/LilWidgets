

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
