using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SandboxApp.Enumerations;
using SandboxApp.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SandboxApp.iOS.Util.NativeNonClientAreaStyler))]
namespace SandboxApp.iOS.Util
{
    public class NativeNonClientAreaStyler : INonClientAreaStyler
    {
        private static UIApplication app;

        public static void Init(UIApplication _app) => app = _app;

        public void SetNonClientArea(Color bgColor, DisplayThemes theme)
        {
            if (theme == DisplayThemes.Light)
                app.SetStatusBarStyle(UIStatusBarStyle.DarkContent, true);
            else
                app.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
        }
    }
}