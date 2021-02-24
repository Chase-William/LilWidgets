using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

using LilWidgets.Widgets;

namespace LilSamples.Pages.LoadingWidgetExamples
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingWidgetTestPage : ContentPage
    {
        public LoadingWidgetTestPage()
        {
            InitializeComponent();
            BindingContext = loadingWidget;
            loadingWidget.SizeChanged += LoadingWidget_SizeChanged;

            On<iOS>().SetUseSafeArea(true);
        }

        private void LoadingWidget_SizeChanged(object sender, EventArgs e)
        {
            loadingWidget.SizeChanged -= LoadingWidget_SizeChanged;
            heightSlider.Maximum = ((Grid)loadingWidget.Parent).Height;
            widthSlider.Maximum = ((Grid)loadingWidget.Parent).Width;
            heightSlider.Value = loadingWidget.Height;
            widthSlider.Value = loadingWidget.Width;
            strokeWidthSlider.Value = LoadingWidget.DEFAULT_STROKE_WIDTH;
        }

        private void heightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => loadingWidget.HeightRequest = e.NewValue;        

        private void widthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => loadingWidget.WidthRequest = e.NewValue;

        private void strokeWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => loadingWidget.StrokeWidth = (float)e.NewValue;
    }
}