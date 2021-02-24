using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

using LilWidgets.Util;

namespace LilWidgets.Lang
{
    public class LimitingSpan
    {
        /// <summary>
        /// The width of the limiting span in pixels.
        /// </summary>
        public float SpanWidthInPixels { get; private set; }
        /// <summary>
        /// The width of the limiting span in the common density unit.
        /// </summary>
        public float SpanWidthWithDensity { get; private set; }
        /// <summary>
        /// Indicates whether the Height is limiting span or the width is the limiting span.
        /// </summary>
        public bool IsHeightTheLimitingSpan { get; private set; }

        
        public LimitingSpan(VisualElement element) // TODO: use a weakreference manager to prevent memory leaks
        {
            element.SizeChanged += Element_SizeChanged;
            
        }

        private void Element_SizeChanged(object sender, EventArgs e)
        {
            var element = (VisualElement)sender;
            // Determine the limiting span, is it the height or width
            IsHeightTheLimitingSpan = element.Height < element.Width;
            // Get the appropriate width
            float width = (float)(IsHeightTheLimitingSpan ? element.Height : element.Width);
            // Update SpanWidth
            SpanWidthInPixels = width * DisplayUtil.DPI;
            SpanWidthWithDensity = width;
        }
    }
}
