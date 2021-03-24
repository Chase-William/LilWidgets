/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using SkiaSharp;

namespace LilWidgets.Widgets
{
    public class LoadingWidget : StrokeWidget
    {
        /// <summary>
        /// The starting position of the arcs.
        /// </summary>
        const float SWEEP_START = -90;

        const float FULL_REVOLUTION = 360f;
        const short MAX_ARC_LENGTH = 359;
        const string ANIMATION_IDENTIFER = "loading";

        const short DEFAULT_ARC_LENGTH = 90;      

        public override void DrawContent(SKCanvas canvas, in SKRectI rect)
        {
            base.DrawContent(canvas, in rect);                     
//#if DEBUG
//            Debug.WriteLine($"Degrees: {baseSweepAngle}");
//#endif
            // Set the shadow for the background arc
            

            // Creating paths
            var backgroundPath = new SKPath();
            backgroundPath.AddArc(FittedRect, -90, 90);

            
            // Draw Calls
            canvas.DrawPath(backgroundPath, ArcPaint); // Background Arc                             
        }
    }
}
