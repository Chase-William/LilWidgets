using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SandboxApp.Pages.ProgressWidgetPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressWidgetPage : ContentPage
    {
        public ProgressWidgetPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            progressWidgetView.Start();
        }

        private void OnGotoSandboxPage_BtnClicked(object sender, System.EventArgs e)
            => Navigation.PushAsync(new ProgressWidgetTestPage());        
    }
}