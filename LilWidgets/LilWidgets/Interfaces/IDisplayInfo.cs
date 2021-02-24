using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Interfaces
{
    /// <summary>
    /// Gets display information from the platform specific project.
    /// </summary>
    public interface IDisplayInfo
    {
        /// <summary>
        /// Gets the DPI for the respective platform.
        /// </summary>
        /// <returns>DPI</returns>
        int GetDisplayDPI();
    }
}
