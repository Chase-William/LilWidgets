using LilWidgets.Widgets;
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
        public LoadingWidgetTestPage()
        {
            InitializeComponent();
            try
            {
                BindingContext = loadingWidget;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            loadingWidget.SizeChanged += OnLoadingWidget_SizeChanged;

            On<iOS>().SetUseSafeArea(true);
        }

        private void OnLoadingWidget_SizeChanged(object sender, EventArgs e)
        {
            loadingWidget.SizeChanged -= OnLoadingWidget_SizeChanged;
            heightSlider.Maximum = ((Layout)loadingWidget.Parent).Height;
            widthSlider.Maximum = ((Layout)loadingWidget.Parent).Width;
            heightSlider.Value = loadingWidget.Height;
            widthSlider.Value = loadingWidget.Width;
        }

        private void OnHeightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => loadingWidget.HeightRequest = e.NewValue;

        private void OnWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => loadingWidget.WidthRequest = e.NewValue;

        private void OnStrokeWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => loadingWidget.StrokeWidthPercentage = (float)e.NewValue;

        private void OnToggleAnimation_BtnClicked(object sender, EventArgs e)
        {
            if (loadingWidget.IsAnimating)
                loadingWidget.Stop();
            else
                loadingWidget.Start();
        }     
    }
}