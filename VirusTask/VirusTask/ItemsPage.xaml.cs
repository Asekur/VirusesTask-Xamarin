using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirusTask.Helper;
using VirusTask.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        private List<Virus> listVirus = new List<Virus>();
        ObservableCollection<VirusInfo> viruses = new ObservableCollection<VirusInfo>();
        public ObservableCollection<VirusInfo> Viruses { get { return viruses; } }

        public ItemsPage()
        {
            InitializeComponent();
            ClickEdit();
            ClickFilters();

            listView.ItemsSource = Viruses;
            listView.RefreshCommand = new Command(() => {
                fetchDataAsync();
                listView.IsRefreshing = false;
            });
            fetchDataAsync();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            fetchDataAsync();
        }

        private void ClickEdit()
        {
            btnEdit.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new EditPage());
                })
            });
        }

        private void ClickFilters()
        {
            btnFilter.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new NavigationPage(new FiltersPage()));
                })
            });
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

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if ((VirusInfo)listView.SelectedItem == null) return;
            VirusInfo virusInfo = (VirusInfo)listView.SelectedItem;
            listView.SelectedItem = null;
            // virusInfo.Virus -  вирус в ячейке
            Navigation.PushModalAsync(new DetailPage(virusInfo.Virus));
        }

    }
}