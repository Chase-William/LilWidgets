using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LilSamples
{
    public partial class MainPage : ContentPage
    {
        const float DEFAULT_EXAMPLE_PERCENT_VALUE = 0.75f;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = progressWidget;
            progressWidget.PercentValue = DEFAULT_EXAMPLE_PERCENT_VALUE;
            percentValueEntry.Text = DEFAULT_EXAMPLE_PERCENT_VALUE.ToString();
            progressWidget.SizeChanged += ProgressWidget_SizeChanged;
        }

        private void ProgressWidget_SizeChanged(object sender, EventArgs e)
        {
            progressWidget.SizeChanged -= ProgressWidget_SizeChanged;
            heightSlider.Maximum = progressWidget.Height;
            widthSlider.Maximum = progressWidget.Width;
            heightSlider.Value = progressWidget.Height;
            widthSlider.Value = progressWidget.Width;            
        }

        private void Apply_BtnClicked(object sender, EventArgs e)
        {
            if (double.TryParse(percentValueEntry.Text, out double result)) // Parse text
            {
                if (result > 1 || result < 0) // Only accept value percentages            
                    return;
                progressWidget.PercentValue = result;
            }          
        }

        private void heightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => progressWidget.HeightRequest = e.NewValue;

        private void widthSlider_ValueChanged(object sender, ValueChangedEventArgs e)
            => progressWidget.WidthRequest = e.NewValue;
    }
}
