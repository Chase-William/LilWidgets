using LilWidgets.Widgets;
using SandboxApp.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SandboxApp
{
    public partial class MainPage : FlyoutPage
    {       
        public MainPage()
        {
            InitializeComponent();
            Detail = GetStyledNavigationPage(typeof(HomePage));
            flyoutPage.FlyoutPageItemsListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutPageItem;
            if (item != null)
            {
                Detail = GetStyledNavigationPage(item.TargetType);          
                flyoutPage.FlyoutPageItemsListView.SelectedItem = null;
                IsPresented = false;
            }
        }

        private NavigationPage GetStyledNavigationPage(Type type)
        {
            return new NavigationPage((Page)Activator.CreateInstance(type))
            {
                BarBackgroundColor = (Color)App.Current.Resources["ColorPrimary"],
                BarTextColor = (Color)App.Current.Resources["ColorContrast"]
            };
        }
    }
}
