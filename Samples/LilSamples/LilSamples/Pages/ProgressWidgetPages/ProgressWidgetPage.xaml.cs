using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LilSamples.Pages.ProgressWidgetPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressWidgetPage : ContentPage
    {
        public ProgressWidgetPage()
        {
            InitializeComponent();
        }

        private void Test_BtnClicked(object sender, System.EventArgs e)
            => Navigation.PushAsync(new ProgressWidgetTestPage());        
    }
}