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
using SandboxApp.Enumerations;
using SandboxApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SandboxApp.Droid.Util.NativeNonClientAreaStyler))]
namespace SandboxApp.Droid.Util
{
    public class NativeNonClientAreaStyler : INonClientAreaStyler
    {
        public static Activity TargetActivity { get; private set; }

        public static void Init(Activity _activity) => TargetActivity = _activity;

        public void SetNonClientArea(Color bgColor, DisplayThemes theme)
        {
            var window = TargetActivity.Window;
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(XFColorToAndroidColor(bgColor));
            TargetActivity.Window.SetNavigationBarColor(XFColorToAndroidColor(bgColor));

            // Update style
            if (theme == DisplayThemes.Light)
                TargetActivity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LightStatusBar | SystemUiFlags.LightNavigationBar);
            else
                TargetActivity.Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
        }

        private Android.Graphics.Color XFColorToAndroidColor(Color color) => Android.Graphics.Color.Rgb((int)(color.R * byte.MaxValue), (int)(color.G * byte.MaxValue), (int)(color.B * byte.MaxValue));
    }
}