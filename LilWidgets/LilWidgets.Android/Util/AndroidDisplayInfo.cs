using LilWidgets.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(LilWidgets.Droid.Util.AndroidDisplayInfo))]
namespace LilWidgets.Droid.Util
{
    /// <summary>
    /// Utility class for getting information about the display.
    /// </summary>
    public class AndroidDisplayInfo : IDisplayInfo
    {
        public AndroidDisplayInfo() { }
        /// <summary>
        /// Implementation for <see cref="IDisplayInfo"/> to get the device specific DPI.
        /// </summary>
        /// <returns>DPI</returns>
        public float GetDisplayDPI()
            => (float)Android.App.Application.Context.Resources.DisplayMetrics.DensityDpi;        
    }
}