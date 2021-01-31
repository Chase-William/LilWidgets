using LilSamples.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilSamples.Interfaces
{
    /// <summary>
    /// Used to interact non-client areas.
    /// </summary>
    public interface INonClientAreaStyler
    {
        /// <summary>
        /// Sets the status bar theme and background color.
        /// </summary>
        /// <param name="bgColor">Color to be set as the background.</param>
        /// <param name="theme">Theme to be set.</param>
        void SetNonClientArea(Xamarin.Forms.Color bgColor, DisplayThemes theme);
    }
}
