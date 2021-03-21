using SkiaSharp;
using LilWidgets.Widgets;

namespace LilWidgets.Interfaces
{
    /// <summary>
    /// An interface that contains the main components for a <see cref="CircularWidget"/>.
    /// </summary>
    public interface ICircularWidgetBindables
    {
        /// <summary>
        /// The color to be used to paint the Arc.
        /// </summary>
        SKColor ArcColor { get; set; }
        /// <summary>
        /// The color of the shadow to be used with the arcs.
        /// </summary>
        SKColor ShadowColor { get; set; }
        /// <summary>
        /// The time in milliseconds it takes for 1 complete cycle of the animation.
        /// </summary>
        uint Duration { get; set; }
        /// <summary>
        /// The target stroke width value to be used for all arcs that make up the widget.
        /// </summary>
        float StrokeWidth { get; set; }
        /// <summary>
        /// Determines the state of the animation.
        /// True == Running, False == Inactive
        /// </summary>
        bool IsAnimating { get; set; }
    }
}
