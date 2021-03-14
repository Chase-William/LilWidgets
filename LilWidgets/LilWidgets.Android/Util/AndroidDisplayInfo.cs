using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
        /// <summary>
        /// Implementation for <see cref="IDisplayInfo"/> to get the device specific DPI.
        /// </summary>
        /// <returns>DPI</returns>
        public int GetDisplayDPI()
        {
            //var logger = LogManager.GetCurrentClassLogger();
            //logger.Info("Android project logging here.");
            Console.WriteLine();
            return (int)Android.App.Application.Context.Resources.DisplayMetrics.DensityDpi;
        }
    }
}