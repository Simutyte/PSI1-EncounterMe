// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using EncounterMe.Services;
using System.Threading.Tasks;

//possible to implement route tracking https://www.xamboy.com/2019/05/17/exploring-map-tracking-ui-in-xamarin-forms/
//https://www.xamboy.com/2019/05/29/google-maps-place-search-in-xamarin-forms/

//TODO - reload page after maps button pressed 

//PROBLEM - doesnt work immediately after enabling location, but works if we change page, then go back to maps


namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {

        public MapPage()
        {
            var permissions = new ChosenPermissions();
            permissions.chosenLocationPermission = false;
            permissions.chosenGpsPermission = false;
            InitializeComponent();
            DisplayCurrentLocation(permissions);
        }


        class ChosenPermissions
        {
            public bool chosenLocationPermission;
            public bool chosenGpsPermission;
        }

        private async void DisplayCurrentLocation(ChosenPermissions permissions)
        {

            //Checks if location permision is enabled, if not, prompts window
            PermissionStatus locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (locationPermissionStatus == PermissionStatus.Granted)
            {
                //Checks if GPS is enabled, if not, prompts a window
                IGpsDependencyService GpsDependency = DependencyService.Get<IGpsDependencyService>();
                bool gpsEnabled = GpsDependency.IsGpsEnabled();
                if (gpsEnabled)
                {
                    try
                    {                        
                        //Trying to display current location
                        var request = new GeolocationRequest(GeolocationAccuracy.Best);
                        var location = await Geolocation.GetLocationAsync(request);

                        if (location != null)
                        {
                            //Moving to location and starting live tracking
                            MoveToLocation(location);
                            TrackingLiveLocation();
                        }
                    }
                    catch (FeatureNotSupportedException featureNotSupportedException)
                    {

                    }
                    catch (FeatureNotEnabledException featureNotEnabledException)
                    {
                        PromptToEnableGPS(permissions);
                    }
                    catch (PermissionException permissionException)
                    {
                        PromptToEnableLocationPermission(permissions);
                    }
                    catch (Exception exception)
                    {

                    }
                }
                //If user was not yet prompted to enable gps
                else if (permissions.chosenGpsPermission == false)
                {
                    PromptToEnableGPS(permissions);
                }
            }
            //If location permission is not already enabled and not yet chosen, prompt the user and call the method once again
            //Otherwise, if the user already denied location permission, he will not get live tracking
            else if (permissions.chosenLocationPermission == false)
            {
                PromptToEnableLocationPermission(permissions);
            }
        }

        async void PromptToEnableLocationPermission(ChosenPermissions permissions)
        {
            permissions.chosenLocationPermission = true;
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            DisplayCurrentLocation(permissions);
        }

        async void PromptToEnableGPS(ChosenPermissions permissions)
        {
            //If there was no button was pressed (tapped outside the alert box), answer = false, user will not get live tracking
            bool answer = await DisplayAlert("Location request", "Turn on your phone's location service for better performance.", "OK", "Maybe later");
            permissions.chosenGpsPermission = true;

            if (answer == true)
            {
                //Opens settings and starts tracking live location
                IGpsDependencyService GpsDependency = DependencyService.Get<IGpsDependencyService>();
                GpsDependency.OpenSettings();
                TrackingLiveLocation();
            }
        }

        //Constantly runs in the background, tracks current location and updates it
        private void TrackingLiveLocation()
        {
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                Task.Run(async () =>
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    var location = await Geolocation.GetLocationAsync(request);
                    MoveToLocation(location);
                });
                return true;
            });
        }

        //Moves map to given location
        void MoveToLocation(Location position)
        {
            Position p = new Position(position.Latitude, position.Longitude);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(1));
            MyMap.MoveToRegion(mapSpan);
        }

        void Button_Clicked(object sender, EventArgs e)
        {

        }

    }
}
