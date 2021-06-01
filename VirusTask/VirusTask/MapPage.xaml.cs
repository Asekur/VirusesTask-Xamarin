using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using VirusTask.Helper;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Pin = Xamarin.Forms.GoogleMaps.Pin;
using PinType = Xamarin.Forms.GoogleMaps.PinType;
using Position = Xamarin.Forms.GoogleMaps.Position;

namespace VirusTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            SetMarkers();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            map.Pins.Clear();
            SetMarkers();
        }

        private void RefreshMapPin(object sender, EventArgs e)
        {
            map.Pins.Clear();
            SetMarkers();
        }

        // SetMarkers -> OnPinClick
        private void OnPinClick(object sender, EventArgs e)
        {
            Pin pin = (Pin)sender;
            List<Virus> list = pin.Tag as List<Virus>;
            if (list != null)
            {
                if (list.Count == 1)
                {
                    Navigation.PushModalAsync(new DetailPage(list[0]));
                } else
                {
                    //go to itemsPage
                    Navigation.PushModalAsync(new FiltersItemsPage(list));
                }
            }
        }

        private async void SetMarkers()
        {
            List<Virus> viruses = await FirebaseHelper.FetchViruses();
            List<string> places = new List<string>();
            foreach (var virus in viruses)
            {
                places.Add(virus.country);
            }

            foreach (var place in places)
            {
                var virusesWithTheSameLocation = viruses.FindAll(virus => (virus.country == place));
                string snippet = "";
                foreach (var vir in virusesWithTheSameLocation)
                {
                    snippet += vir.fullName + "\n";
                }

                try
                {
                    var locations = await Geocoding.GetLocationsAsync(place);
                    //variants location
                    var location = locations?.FirstOrDefault();
                    if (location != null)
                    {
                        Pin pin = new Pin
                        {
                            Label = place,
                            Address = snippet,
                            Type = PinType.Place,
                            Position = new Position(location.Latitude, location.Longitude)
                        };

                        pin.Tag = virusesWithTheSameLocation;
                      
                        pin.Clicked += OnPinClick;
                        map.Pins.Add(pin);
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    Console.WriteLine("Feature: " + fnsEx);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}