using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets
{
    /// <summary>
    /// Entry-Point for the library.
    /// </summary>
    public class LilWidgets
    {
        /// <summary>
        /// Contains the Dots-Per-Inch given by the platform specific projects from the client.
        /// </summary>
        internal static float DPI { get; private set; }

        /// <summary>
        /// Initializes the Library.
        /// </summary>
        /// <param name="dpi">The DPI to be by the Widgets.</param>
        public static void Init(float dpi) => DPI = dpi;
    }
}
