// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using EncounterMe.Pins;
using System.Collections.Generic;

namespace EncounterMe.Views
{

    //Needs implementation for location visited

    // User user = User.Instance;
    //user.SetMyVisitedObjects(_pin);


    delegate bool IsCloseEnough(double distance);

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndividualObjectPage : ContentPage
    {
        public delegate bool Filter(double x);
        private MapPin _pin;
        
        public IndividualObjectPage(MapPin pinToRender)
        {
            InitializeComponent();
            PinsList list = PinsList.GetPinsList();
            _pin = pinToRender;
            this.BindingContext = pinToRender;

            CloseObjects.Text = AllObjects(list.ListOfPins, _pin, IsClose);
            AwayObjects.Text = AllObjects(list.ListOfPins, _pin, IsAway);
            FarAwayObjects.Text = AllObjects(list.ListOfPins, _pin, IsFarAway);
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
                    Ats.Text = Calculating.GetDistanceInMeters( currentLocation, _pin).ToString();
                else if (selectedIndex == 1)
                    Ats.Text = Calculating.GetDistanceInKm( currentLocation, _pin).ToString();
                else if (selectedIndex == 2)
                    Ats.Text = Calculating.GetDistanceInYards( currentLocation, _pin).ToString();
                else
                    Ats.Text = Calculating.GetDistanceInMiles( currentLocation, _pin).ToString();
            }
            else
                Ats.Text = "";
        }
        
        

        static string AllObjects(List<MapPin> list, MapPin currentPin, Filter filter)
        {
            string Objects = "";
            Location loc = new Location() { Latitude = currentPin.Latitude, Longitude = currentPin.Longitude };

            foreach (MapPin pin in list)
            {
                var distance = Calculating.GetDistanceInKm(loc, pin);
                if (filter(distance) && pin.Id != currentPin.Id)
                {
                    Objects += pin.Name + ' ' + Math.Round(distance, 2).ToString() + " km" + '\n';
                }
            }

            if (String.IsNullOrWhiteSpace(Objects))
            {
                return "There is none";
            }
            else
                return Objects;
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

        static bool IsClose(double x)
        {
            return x <= 20;
        }

        static bool IsAway(double x)
        {
            return (x > 20 && x <=100);
        }

        static bool IsFarAway(double x)
        {
            return x > 100;
        }
    }

    
    
}
