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
    public partial class LoadingWidgetPage : ContentPage
    {
        public LoadingWidgetPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadingWidget.IsAnimating = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            loadingWidget.IsAnimating = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
            => this.Navigation.PushAsync(new LoadingWidgetTestPage());        
    }
}