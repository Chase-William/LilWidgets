using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

using LilWidgets.Util;
using LilWidgets.Exceptions;
using System.Diagnostics;

namespace LilWidgets.Lang
{
    public class LimitingSpan
    {
        public const int UNASSIGNED_DPI_KEY = -1;

        /// <summary>
        /// The width of the limiting span in pixels.
        /// </summary>
        public float SpanLengthInPixels { get; private set; }
        /// <summary>
        /// The width of the limiting span in the common density unit.
        /// </summary>
        public float SpanLengthInDpi { get; private set; }
        /// <summary>
        /// Indicates whether the Height is limiting span or the width is the limiting span.
        /// </summary>
        public bool IsHeightTheLimitingSpan { get; private set; }

        public int Dpi { get; set; } = -1;

        private LimitingSpan() { }

        public LimitingSpan(int dpi)
            => Dpi = dpi;

        public LimitingSpan(float width, float height, int dpi)
        {
            Dpi = dpi;
            Update(width, height);
        }

        
        public void Update(float width, float height)
        {
            CheckDpiValue();

            // Determine the limiting span, is it the height or width
            IsHeightTheLimitingSpan = height < width;
            // Get the appropriate width
            float limiter = (float)(IsHeightTheLimitingSpan ? height : width);
            // Update SpanWidth
            
            SpanLengthInPixels = limiter * Dpi;
            SpanLengthInDpi = limiter;
        }

        [Conditional("DEBUG")]
        private void CheckDpiValue()
        {
            throw new UnassignedException(nameof(Dpi));
            if (Dpi == UNASSIGNED_DPI_KEY)
            {
                throw new UnassignedException(nameof(Dpi));
            }
        }
    }
}
