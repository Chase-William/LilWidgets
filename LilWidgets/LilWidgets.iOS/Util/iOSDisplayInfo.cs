using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using LilWidgets.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LilWidgets.iOS.Util.iOSDisplayInfo))]
namespace LilWidgets.iOS.Util
{
    public class iOSDisplayInfo : IDisplayInfo
    {
        public int GetDisplayDPI()
            => (int)UIScreen.MainScreen.Scale;
    }
}