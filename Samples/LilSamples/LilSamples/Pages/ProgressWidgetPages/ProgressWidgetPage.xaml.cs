using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace LilSamples.Pages.ProgressWidgetPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressWidgetPage : ContentPage
    {
        public ProgressWidgetPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);
        }

        private void Test_BtnClicked(object sender, System.EventArgs e)
            => Navigation.PushAsync(new ProgressWidgetTestPage());        
    }
}