using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTask.Helper;
using VirusTask.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static VirusTask.App;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltersItemsPage : ContentPage
    {
        private List<Virus> listVirus = new List<Virus>();
        ObservableCollection<VirusInfo> viruses = new ObservableCollection<VirusInfo>();
        public ObservableCollection<VirusInfo> Viruses { get { return viruses; } }

        public FiltersItemsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public FiltersItemsPage(List<Virus> viruses)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.BackgroundColor = App.AppTheme == Theme.Dark ? Color.FromHex("#E1E1E1") : Color.FromHex("#2B2B2B");

            listView.ItemsSource = Viruses;
            listView.RefreshCommand = new Command(() => {
             //   fetchDataAsync();
                listView.IsRefreshing = false;
            });
            listVirus = viruses;
            AddVirusInfo(viruses);
        }

        private async void fetchDataAsync()
        {
            var items = await FirebaseHelper.FetchViruses();
            listVirus.Clear();
            listVirus = items;
            AddVirusInfo(items);
        }

        private void AddVirusInfo(List<Virus> items)
        {
            viruses.Clear();
            foreach (var item in items)
            {
                viruses.Add(new VirusInfo(item));
            }
        }

        private void OnItemFilterSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if ((VirusInfo)listView.SelectedItem == null) return;
            VirusInfo virusInfo = (VirusInfo)listView.SelectedItem;
            listView.SelectedItem = null;
            // virusInfo.Virus -  вирус в ячейке
            Navigation.PushModalAsync( new NavigationPage (new DetailPage(virusInfo.Virus)));
        }
    }
}