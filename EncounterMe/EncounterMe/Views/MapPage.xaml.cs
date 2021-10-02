// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Diagnostics;
//using EncounterMe.Droid;
using EncounterMe.Services;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {

        //public bool LocationSettingsButtonPressed = false;

        public MapPage()
        {
            InitializeComponent();
            DisplayCurrentLocation();
            
        }

        private async void DisplayCurrentLocation()
        {

            PermissionStatus locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (locationPermissionStatus == PermissionStatus.Granted)
            {

                var knownLocation = await Geolocation.GetLastKnownLocationAsync();

                if (knownLocation != null)
                {
                    MoveToLocation(knownLocation);
                    //Debug.WriteLine($"Latitude: {knownLocation.Latitude}, Longitude: {knownLocation.Longitude}, Altitude: {knownLocation.Altitude}");
                }
                else
                {

                    try
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Best);
                        var location = await Geolocation.GetLocationAsync(request);

                        if (location != null)
                        {
                            MoveToLocation(location);
                        }
                    }
                    catch (FeatureNotSupportedException featureNotSupportedException)
                    {

                    }
                    catch (FeatureNotEnabledException featureNotEnabledException)
                    {
                        
                        IGpsDependencyService GpsDependency = DependencyService.Get<IGpsDependencyService>();
                        bool gpsEnabled = GpsDependency.IsGpsEnabled();
                        if (!gpsEnabled)
                        {

                            bool answer = await DisplayAlert("Alert", "Turn on your phone's location service for better performance.", "OK", "Maybe later");
                            if (answer == true)
                            {
                                //LocationSettingsButtonPressed = true;
                                GpsDependency.OpenSettings();
                                DisplayCurrentLocation();
                            }

                        }

                    }
                    catch (PermissionException permissionException)
                    {
                        AppInfo.ShowSettingsUI();
                        DisplayCurrentLocation();
                    }
                    catch (Exception exception)
                    {

                    }
                }
            }
            else
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                DisplayCurrentLocation();
            }
        }

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
