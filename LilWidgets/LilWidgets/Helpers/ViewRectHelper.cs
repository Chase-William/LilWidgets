﻿using LilWidgets.Enumerations;
using LilWidgets.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Components
{
    internal class ViewRectHelper
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewRectHelper"/> class.
        /// </summary>
        public ViewRectHelper() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewRectHelper"/> class and passes
        /// the given parameters to the <see cref="Update(float, float)"/> method for invocation.
        /// </summary>
        /// <param name="width">The width of a given view.</param>
        /// <param name="height">The height of a given view.</param>
        public ViewRectHelper(float width, float height)
            => Update(width, height);

        /// <summary>
        /// Indicates whether the width and height are equal in length or which is the smaller of the two.
        /// </summary>
        public ViewSpans LimitingSpan { get; private set; }

        /// <summary>
        /// Length of the limiting span given in the <see cref="Update(float, float)"/> method parameters.
        /// </summary>
        public float LimitingSpanLength { get; private set; }

        /// <summary>
        /// Takes the width and height as parameters to update the encapsulating classes properties.
        /// The to be updated properties include <see cref="LimitingSpan"/> and <see cref="LimitingSpanLength"/>.
        /// </summary>
        /// <param name="width">The width of a given view.</param>
        /// <param name="height">The height of a given view.</param>
        public void Update(float width, float height)
        {            
            if (height > width) // The width is the smaller span of the two
            {
                LimitingSpan = ViewSpans.Width;
                LimitingSpanLength = width;
            }
            else if (height < width) // The height is the smaller span of the two
            {

                LimitingSpan = ViewSpans.Height;
                LimitingSpanLength = height;
            }
            else // The width and height are equal in value
            {
                LimitingSpan = ViewSpans.Equal;
                LimitingSpanLength = width;
            }
        }
    }
}
