using SandboxApp.Pages;
using SandboxApp.Pages.LoadingWidgetExamples;
using SandboxApp.Pages.ProgressWidgetPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SandboxApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutMenuPage : ContentPage
    {
        public ListView FlyoutPageItemsListView { get; private set; }

        public FlyoutMenuPage()
        {
            InitializeComponent();
            BindingContext = new FlyoutMenuViewModel();
            FlyoutPageItemsListView = pageItemsListView;
        }        
    }

    public class FlyoutMenuViewModel
    {
        public ObservableCollection<FlyoutPageItem> MenuItems { get; set; }

        public FlyoutMenuViewModel()
        {
            MenuItems = new ObservableCollection<FlyoutPageItem>(new[]
            {
                new FlyoutPageItem { Id = 0, Title = "Home", TargetType = typeof(HomePage) },
                new FlyoutPageItem { Id = 1, Title = "Progress Widget", TargetType = typeof(ProgressWidgetPage) },
                new FlyoutPageItem { Id = 2, Title = "Loading Widget", TargetType = typeof(LoadingWidgetPage) },
            });
        }
    }
}