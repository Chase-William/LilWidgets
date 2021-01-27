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
    }
}
