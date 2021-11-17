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
        public delegate bool Filter(double x, int i);
        private MapPin _pin;
        private List<MapPin> list;
        
        public IndividualObjectPage(MapPin pinToRender)
        {
            InitializeComponent();
            PinsList PinList = PinsList.GetPinsList();
            list = PinList.ListOfPins;
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

                Ats.Text = GetDistanceByIndex(selectedIndex, currentLocation, _pin).ToString();

                CloseObjects.Text = AllObjects(list, _pin, selectedIndex, IsClose);
                AwayObjects.Text = AllObjects(list, _pin, selectedIndex, IsAway);
                FarAwayObjects.Text = AllObjects(list, _pin, selectedIndex, IsFarAway);

                /*
                if (selectedIndex == 0)
                    Ats.Text = Calculating.GetDistanceInMeters( currentLocation, _pin).ToString();
                else if (selectedIndex == 1)
                    Ats.Text = Calculating.GetDistanceInKm( currentLocation, _pin).ToString();
                else if (selectedIndex == 2)
                    Ats.Text = Calculating.GetDistanceInYards( currentLocation, _pin).ToString();
                else
                    Ats.Text = Calculating.GetDistanceInMiles( currentLocation, _pin).ToString();*/
            }
            else
            {
                Ats.Text = "";
                CloseObjects.Text = "Please pick unit of measurement";
                AwayObjects.Text = "Please pick unit of measurement";
                FarAwayObjects.Text = "Please pick unit of measurement";
            }
                
        }

        public static double? GetDistanceByIndex(int i, Location loc, MapPin _pin)
        {
            if (i == 0)
                return Calculating.GetDistanceInMeters(loc, _pin);
            else if (i == 1)
                return Calculating.GetDistanceInKm(loc, _pin);
            else if (i == 2)
                return Calculating.GetDistanceInYards(loc, _pin);
            else if (i == 3)
                return Calculating.GetDistanceInMiles(loc, _pin);
            else
                return null;
        }
        

        static string AllObjects(List<MapPin> list, MapPin currentPin, int i, Filter filter)
        {
            string Objects = "";
            Location loc = new Location() { Latitude = currentPin.Latitude, Longitude = currentPin.Longitude };

            foreach (MapPin pin in list)
            {
                var distance = GetDistanceByIndex(i, loc, pin);
                if (filter((double)distance, i) && pin.Id != currentPin.Id)
                {
                    Objects += pin.Name + ' ' + Math.Round((double)distance, 2).ToString() + '\n';
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

        static bool IsClose(double x, int i)
        {
            if (i == 0)
                return x <= 20000;

            return x <= 20;
        }

        static bool IsAway(double x,int i)
        {
            if(i == 0)
                return (x > 20000 && x <= 100000);

            return (x > 20 && x <=100);
        }

        static bool IsFarAway(double x, int i)
        {
            if (i == 0)
                return x > 100000;
            return x > 100;
        }
    }

    
    
}
