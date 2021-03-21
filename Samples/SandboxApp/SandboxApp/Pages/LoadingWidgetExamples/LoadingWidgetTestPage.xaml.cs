using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;


namespace SandboxApp.Pages.LoadingWidgetExamples
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingWidgetTestPage : ContentPage
    {
        //public LoadingWidgetTestPage()
        //{
        //    InitializeComponent();
        //    try
        //    {
        //        BindingContext = loadingWidget;
        //    }            
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine();
        //    }
        //    loadingWidget.SizeChanged += OnLoadingWidget_SizeChanged;

        //    On<iOS>().SetUseSafeArea(true);
        //}

        //private void OnLoadingWidget_SizeChanged(object sender, EventArgs e)
        //{
        //    loadingWidget.SizeChanged -= OnLoadingWidget_SizeChanged;
        //    heightSlider.Maximum = ((Grid)loadingWidget.Parent).Height;
        //    widthSlider.Maximum = ((Grid)loadingWidget.Parent).Width;
        //    heightSlider.Value = loadingWidget.Height;
        //    widthSlider.Value = loadingWidget.Width;
        //    //strokeWidthSlider.Value = LoadingWidget.DEFAULT_STROKE_WIDTH;
        //}

        //private void OnHeightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        //    => loadingWidget.HeightRequest = e.NewValue;        

        //private void OnWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        //    => loadingWidget.WidthRequest = e.NewValue;

        ////private void OnStrokeWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        ////    => loadingWidget.StrokeWidth = (float)e.NewValue;

        //private void OnToggleAnimation(object sender, EventArgs e)
        //{
        //    loadingWidget.IsAnimating = !loadingWidget.IsAnimating;
        //}
    }
}