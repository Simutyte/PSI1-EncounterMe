// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadRoutePage : ContentPage
    {

        IEnumerable<MapPin> _mapPins;
        public static IEnumerable<MapPin> PinsToRender { get; set; }

        public LoadRoutePage(IEnumerable<MapPin> mapPins)
        {
            InitializeComponent();
            _mapPins = mapPins;
            RoutesListView.ItemsSource = GetSortedMapPins();
        }

        private IEnumerable<MapPin> GetSortedMapPins()
        {
            CalculateDistances();
            var objectsQueryOrderedByDistance = _mapPins.OrderBy(pin => pin.DistanceToUser);

            //objectsQueryOrderedByDistance.First().DistanceBetweenPoints = "Start point";

            return objectsQueryOrderedByDistance;
        }

        public async void CalculateDistances()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            var location = await Geolocation.GetLocationAsync(request);

            foreach (var pin in _mapPins)
            {
                pin.DistanceToUser = Location.CalculateDistance(location.Latitude, location.Longitude,
                                                                pin.Latitude, pin.Longitude, DistanceUnits.Kilometers);
            }
        }

        async void listView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            var pinToPass = ((ListView)sender).SelectedItem as MapPin;
            if (pinToPass == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushAsync(new IndividualObjectPage(pinToPass));
            
        }
        void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void Display_Pins_On_Map(object sender, EventArgs e)
        {
            PinsToRender = _mapPins;
            await AppShell.Current.GoToAsync($"//home/tab/MapPage?route=true");
        }

        private async void Display_Route_On_Map(object sender, EventArgs e)
        {
            _mapPins.OrderBy(x => x.DistanceToUser);
            var pin = _mapPins.Where(x => x.Latitude != 0 && x.Longitude != 0).FirstOrDefault();
            await AppShell.Current.GoToAsync($"//home/tab/MapPage?lat={pin.Latitude}&longi={pin.Longitude}&drawing=true");
        }
    }
}
