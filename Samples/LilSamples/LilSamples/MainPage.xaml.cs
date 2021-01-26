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
        public MainPage()
        {
            InitializeComponent();
        }

        private void Replay_BtnClicked(object sender, EventArgs e)
        {
            progressWidget.PercentValue = progressWidget.PercentValue == .75d ? 0 : .75d;
        }
    }
}
