using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LilWidgets.Widgets;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SandboxApp.Pages.ProgressWidgetPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressWidgetTestPage : ContentPage
    {
        const float DEFAULT_EXAMPLE_PERCENT_VALUE = 0.75f;

        public ProgressWidgetTestPage()
        {
            InitializeComponent();
            BindingContext = progressWidget;
            progressWidget.ProgressPercentage = DEFAULT_EXAMPLE_PERCENT_VALUE;
            percentValueEntry.Text = DEFAULT_EXAMPLE_PERCENT_VALUE.ToString();
            progressWidget.SizeChanged += OnProgressWidget_SizeChanged;
            strokeWidthSlider.Value = StrokeWidget.DEFAULT_STROKE_WIDTH_PERCENTAGE;
            textWidthPercentageSlider.Value = ProgressWidget.DEFAULT_TEXT_SIZE_PERCENTAGE;
            On<iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            progressWidget.Start();
            progressWidget.AutoAnimate = true;
        }

        private void OnProgressWidget_SizeChanged(object sender, EventArgs e)
        {
            progressWidget.SizeChanged -= OnProgressWidget_SizeChanged;
            heightSlider.Maximum = ((Grid)progressWidget.Parent).Height;
            widthSlider.Maximum = ((Grid)progressWidget.Parent).Width;
            heightSlider.Value = progressWidget.Height;
            widthSlider.Value = progressWidget.Width;
        }
        private void OnApply_BtnClicked(object sender, EventArgs e)
        {
            if (float.TryParse(percentValueEntry.Text, out float result)) // Parse text
            {
                if (result > 1 || result < 0) // Only accept value percentages            
                    return;
                progressWidget.ProgressPercentage = result;
            }
        }
        private void OnHeightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => progressWidget.HeightRequest = e.NewValue;
        private void OnWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => progressWidget.WidthRequest = e.NewValue;
        private void OnStrokeWidthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => progressWidget.StrokeWidthPercentage = (float)e.NewValue;
        private void OnTextSizePercentage_ValueChanged(object sender, ValueChangedEventArgs e)
            => progressWidget.TextSizePercentage = (float)e.NewValue;
    }
}