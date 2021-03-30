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
            await OpenGithubPageInBrowser();
        }        

        private async Task OpenGithubPageInBrowser()
        {
            await Browser.OpenAsync("https://github.com/ChaseRoth/LilWidgets");
            Device.BeginInvokeOnMainThread(() => githubBtn.IsEnabled = true);
        }       
    }
}