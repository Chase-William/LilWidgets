using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Components
{
    internal interface ISquareArcOuterRectComponent : ILimitingSpanProviderComponent
    {
        public SKRect Rect { get; protected set; }

        //public void UpdateOuterRect()
        //{
        //    if (viewRectable.LimitingSpan == Enumerations.ViewSpans.Height) // Canvas is wider than it is tall, so compute for height
        //    {
        //        arcRect = new SKRect(midX - midY + halfShadowStrokeWidth, // left
        //                             halfShadowStrokeWidth, // top
        //                             midX + midY - halfShadowStrokeWidth, // right
        //                             info.Height - halfShadowStrokeWidth); // bottom
        //    }
        //    else // Canvas is taller than it is wide so compute for width
        //    {
        //        arcRect = new SKRect(halfShadowStrokeWidth, // left
        //                             midY - midX + halfShadowStrokeWidth, // top
        //                             info.Width - halfShadowStrokeWidth, // right
        //                             midY + midX - halfShadowStrokeWidth); // bottom
        //    }
        //}
    }
}
