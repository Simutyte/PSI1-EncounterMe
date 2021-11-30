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
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            Location location = await Geolocation.GetLocationAsync(request);
            var first = _mapPins.Where(x => x.Latitude!= 0 && x.Longitude != 0).FirstOrDefault();
            double minDistance = Location.CalculateDistance(location.Latitude, location.Longitude, first.Longitude, first.Longitude, DistanceUnits.Kilometers);
            double distance;
            MapPin pinForRoute = new MapPin();

            //Finds the closest pin to current location
            foreach (MapPin pin in _mapPins)
            {
                distance = Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Kilometers);
                if (distance < minDistance && distance != 0)
                {
                    minDistance = distance;
                    pinForRoute = pin;
                }
            }

            await AppShell.Current.GoToAsync($"//home/tab/MapPage?lat={pinForRoute.Latitude}&longi={pinForRoute.Longitude}&drawing=true");
        }
    }
}
