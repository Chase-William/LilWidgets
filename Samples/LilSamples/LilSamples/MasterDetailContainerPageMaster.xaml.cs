using LilSamples.Pages.ProgressWidgetPages;
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

namespace LilSamples
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailContainerPageMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailContainerPageMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetailContainerPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterDetailContainerPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailContainerPageMasterMenuItem> MenuItems { get; set; }

            public MasterDetailContainerPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetailContainerPageMasterMenuItem>(new[]
                {
                    new MasterDetailContainerPageMasterMenuItem { Id = 0, Title = "Progress Widget", TargetType = typeof(ProgressWidgetPage) },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}