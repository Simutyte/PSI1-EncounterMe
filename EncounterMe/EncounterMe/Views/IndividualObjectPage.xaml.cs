// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EncounterMe.Views;
using Xamarin.Essentials;
using EncounterMe.Users;

namespace EncounterMe.Views
{

    //Needs implementation for location visited

    // User user = User.Instance;
    //user.SetMyVisitedObjects(_pin);


    delegate bool IsCloseEnough(double distance);

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndividualObjectPage : ContentPage
    {
        private MapPin _pin;
        public IndividualObjectPage(MapPin pinToRender)
        {
            InitializeComponent();
            _pin = pinToRender;
            this.BindingContext = pinToRender;
        }

        //Calculates distance from current location to pin location and 
        private async void CheckIn_Clicked(object sender, EventArgs e)
        {

            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var currentLocation = await Geolocation.GetLocationAsync(request);
            double distance = Location.CalculateDistance(currentLocation.Latitude, currentLocation.Longitude, _pin.Latitude, _pin.Longitude, DistanceUnits.Kilometers);

            IsCloseEnough isCloseEnough = new IsCloseEnough(Allow);
            IsCloseEnough isCloseEnough1 = new IsCloseEnough(delegate(double dist)
            {
                return distance <= 20 ? true : false;
            });

            if (isCloseEnough(distance))
                await DisplayAlert("Congratulations!", "Object added to visited objects list", "Ok");
            else
                await DisplayAlert("Alert", "You are too far away from the location", "Ok");
        }


        public static bool Allow(double distance)
        {
            return distance <= 20 ? true : false;
        }


        private async void Go_Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
            
        }

        private async void Display_Route_On_Map(object sender, EventArgs e)
        {
            //Location location = new Location { Latitude = _pin.Latitude, Longitude = _pin.Longitude };
            Location location = new Location { Latitude = 38.01655470103673, Longitude = -121.88968844314147 };
            double lat = 38.01655470103673;
            double longi = -121.88968844314147;
            await AppShell.Current.GoToAsync($"//home/tab/MapPage?lat={lat}&longi={longi}");
        }

    }
}
