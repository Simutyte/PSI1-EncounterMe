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
using EncounterMe.Pins;


//TODO update pins on appearing

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadRoutePage : ContentPage
    {

        IEnumerable<MapPin> _mapPins;
        public static IEnumerable<MapPin> PinsToRender { get; set; }

        private string CreatorsNames { get; set; }
        public LoadRoutePage(IEnumerable<MapPin> mapPins)
        {
            InitializeComponent();
            _mapPins = mapPins;
            RoutesListView.ItemsSource = GetSortedMapPins();

           ForCreators.Text = "Thanks to the creators: " + CreatorsNames;
        }

        private IEnumerable<MapPin> GetSortedMapPins()
        {
            CalculateDistances();
            var objectsQueryOrderedByDistance = _mapPins.OrderBy(pin => pin.DistanceToUser);

            //objectsQueryOrderedByDistance.First().DistanceBetweenPoints = "Start point";
            var creators = from pin in objectsQueryOrderedByDistance
                           join user in App.s_mapPinService.AllUsers
                           on pin.CreatorId equals user.Id
                           select user.Username ;
             
            CreatorsNames = creators.Distinct().Aggregate((concat, str) => $"{concat}, {str}");

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

        //galima paturbint sita metoda bsk, kad tiksliau pasakytu, arba renderint tik tuos, kuriu neaplanke
        private async void Display_Route_On_Map(object sender, EventArgs e)
        {
            CalculateDistances();
            PinsToRender = _mapPins.OrderBy(x => x.DistanceToUser);

            bool alreadyAlerted = false;
            foreach (var pin in PinsToRender)
            {
                if (pin.Visited && !alreadyAlerted)
                {
                    alreadyAlerted = true;
                    bool ans = await DisplayAlert("Alert", "This route includes places, that you have already visited.\nIf you choose to go this route, those pins will be reset", "Ok", "No");

                    if (!ans)
                    {
                        return;
                    }
                }
                pin.Visited = false;
            }

            await AppShell.Current.GoToAsync($"//home/tab/MapPage?drawing=true&route=true");
        }
    }
}
