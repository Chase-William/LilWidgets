using System;
using LilWidgets.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LilWidgets.iOS.Util.IOSDisplayInfo))]
namespace LilWidgets.iOS.Util
{
    public class IOSDisplayInfo : IDisplayInfo
    {
        public IOSDisplayInfo() { }

        public float GetDisplayDPI()
        {
            try
            {
                return (float)UIScreen.MainScreen.Scale;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                return -1;
            }
        }
    }
}