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
    }
}
