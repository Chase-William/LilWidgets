using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Structures
{
    public struct Thickness
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }       

        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Thickness(float leftAndRight, float topAndBottom)
        {
            Left = leftAndRight;
            Right = leftAndRight;
            Top = topAndBottom;
            Bottom = topAndBottom;
        }

        public Thickness(float univeral)
        {
            Left = univeral;
            Top = univeral;
            Right = univeral;
            Bottom = univeral;
        }
    }
}
