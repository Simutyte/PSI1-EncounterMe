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

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndividualObjectPage : ContentPage
    {

        public IndividualObjectPage(MapPin pinToRender)
        {
            InitializeComponent();
            SetPinLocation(pinToRender);
            this.BindingContext = pinToRender;
        }




        public Location _pinLocation;
        public void SetPinLocation(MapPin pin)
        {
            _pinLocation.Latitude = pin.Location.Latitude;
            _pinLocation.Longitude = pin.Location.Longitude;
        }


        //Calculates distance from current location to pin location and 
        private async void CheckIn_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Wer are in");
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var currentLocation = await Geolocation.GetLocationAsync(request);
            Console.WriteLine("Got location");
            Console.WriteLine("Passed");
            Console.WriteLine(_pinLocation.Latitude.ToString());
            double distance = Location.CalculateDistance(currentLocation.Latitude, currentLocation.Longitude, _pinLocation.Latitude, _pinLocation.Longitude, DistanceUnits.Kilometers);
            Console.WriteLine("???");
            if (distance > 0.02)
            {
                double distanceFromPin = distance - 0.02;
                await DisplayAlert("Alert", "You are " + distanceFromPin + " meters off", "Ok");
            }
            else
            {
                //Needs implementation
                //visited = 1
                await DisplayAlert("Congratulations!", "Object added to visited objects list", "Ok");
            }
        }
    }
}
