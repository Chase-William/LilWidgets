using LilWidgets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace LilWidgets.Util
{
    /// <summary>
    /// Gets display information from the current platform using <see cref="DependencyService"/>.
    /// </summary>
    public static class DisplayUtil
    {
        /// <summary>
        /// Gets the current platform's DPI.
        /// </summary>
        public static int DPI { get; private set; }

        static DisplayUtil()
        {
            var service = DependencyService.Get<IDisplayInfo>(DependencyFetchTarget.NewInstance);
            if (service == null)
            {
                throw new Exception("Make sure you have added the LilWidgets library to your platform specific projects!");
            }
            DPI = service.GetDisplayDPI();          
        }
    }
}
