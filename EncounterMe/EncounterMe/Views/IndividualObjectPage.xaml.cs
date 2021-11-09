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

            //Checks if you are within 20m of the specified place
            if (distance > 0.02)
            {
                double distanceFromPin = distance - 0.02;
                await DisplayAlert("Alert", "You are " + (int)distanceFromPin + " meters off", "Ok");
            }
            else
            {
                //Needs implementation
                //visited = 1

               // User user = User.Instance;
                //user.SetMyVisitedObjects(_pin);

                await DisplayAlert("Congratulations!", "Object added to visited objects list", "Ok");
            }
        }


        private async void Go_Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
            
        }
    }
}
