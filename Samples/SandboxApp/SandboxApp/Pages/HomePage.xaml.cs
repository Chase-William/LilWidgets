using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Essentials;

using System.Threading.Tasks;

namespace SandboxApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
            => InitializeComponent();

        private async void OnOpenGithub_BtnClicked(object sender, EventArgs e)
        {
            githubBtn.IsEnabled = false;
            await ((VisualElement)sender).ScaleTo(1.1, 200, Easing.CubicInOut);
            await OpenGithubPageInBrowser();
            await ((VisualElement)sender).ScaleTo(1.0, 200, Easing.CubicInOut);
        }

        private async Task OpenGithubPageInBrowser()
        {
            await Browser.OpenAsync("https://github.com/ChaseRoth/LilWidgets");
            Device.BeginInvokeOnMainThread(() => githubBtn.IsEnabled = true);
        }
    }
}