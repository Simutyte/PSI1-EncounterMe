// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using EncounterMe.Pins;

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

       
        public async void selected_measurement(object sender, EventArgs e)
        {
            int selectedIndex = MeasurementPicker.SelectedIndex;
            Console.WriteLine(selectedIndex);
            if (selectedIndex != -1)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var currentLocation = await Geolocation.GetLocationAsync(request);
                if (selectedIndex == 0)
                    Ats.Text = GetDistance(MetersDelegate, currentLocation, _pin);
                else if (selectedIndex == 1)
                    Ats.Text = GetDistance(KilometersDelegate, currentLocation, _pin);
                else if (selectedIndex == 2)
                    Ats.Text = GetDistance(YardsDelegate, currentLocation, _pin);
                else
                    Ats.Text = GetDistance(MilesDelegate, currentLocation, _pin);
            }
            else
                Ats.Text = "";
        }
        

        Delegate KilometersDelegate = Calculating.GetDistanceInKm;
        Delegate MilesDelegate = Calculating.GetDistanceInMiles;
        Delegate MetersDelegate = Calculating.GetDistanceInM;
        Delegate YardsDelegate = Calculating.GetDistanceInYards;

        static string GetDistance(Delegate d, Location loc, MapPin pin)
        {
            return d(loc, pin).ToString();
        }

        //Calculates distance from current location to pin location and 
        private async void CheckIn_Clicked(object sender, EventArgs e)
        {

            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var currentLocation = await Geolocation.GetLocationAsync(request);
            double distance = Location.CalculateDistance(currentLocation.Latitude, currentLocation.Longitude, _pin.Latitude, _pin.Longitude, DistanceUnits.Kilometers);

            IsCloseEnough isCloseEnough = new IsCloseEnough(Allow);


            IsCloseEnough isCloseEnough1 = new IsCloseEnough(delegate (double dist)
            {
                return distance <= 0.02;
            });


            if (isCloseEnough(distance))
                await DisplayAlert("Congratulations!", "Object added to visited objects list", "Ok");
            else
                await DisplayAlert("Alert", "You are too far away from the location", "Ok");
        }


        public static bool Allow(double distance)
        {
            return distance <= 0.02;
        }


        private async void Go_Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
            
        }

        private async void Display_Route_On_Map(object sender, EventArgs e)
        {
            //Used for testing
            //double lat = 38.01655470103673,
            //double longi = -121.88968844314147

            //Real
            double lat = _pin.Latitude;
            double longi = _pin.Longitude;

            await AppShell.Current.GoToAsync($"//home/tab/MapPage?lat={lat}&longi={longi}&drawing=true");
        }

    }

    public delegate double Delegate(Location location, MapPin pin);
    
}
