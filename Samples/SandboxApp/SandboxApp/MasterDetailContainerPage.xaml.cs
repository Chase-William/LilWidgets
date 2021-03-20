using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SandboxApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailContainerPage : MasterDetailPage
    {
        public MasterDetailContainerPage()
        {
            InitializeComponent();         
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterDetailContainerPageMasterMenuItem;
            if (item == null)
                return;
            Page page = null;
            try
            {
                page = (Page)Activator.CreateInstance(item.TargetType);
            }            
            catch(Exception ex)
            {
                Console.WriteLine();
            }
            page.Title = item.Title;

            Detail = new NavigationPage(page)
            {
                BarBackgroundColor = (Color)App.Current.Resources["ColorPrimary"],
                BarTextColor = (Color)App.Current.Resources["ColorContrast"]
            };
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}