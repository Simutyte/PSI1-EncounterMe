// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace EncounterMe.Pins
{
    class CheckAddressCommands
    {
        private string _address { get; set; }
        private Location _location { get; set; }

        private bool _existAddress { get; set; }
        private string _city { get; set; }

        public CheckAddressCommands() { }

        public string GetAddress(Location location)
        {
            _location = location;
            GetAddressFromCoordinates();
            return _address;
        }

        public Location GetCoordinates(string address)
        {
            _address = address;
            GetCoordinatesFromAddress();
            return _location;
        }

        public string GetCity(Location location)
        {
            _location = location;
            GetCityFromCoordinates();
            return _city;
        }

        public bool CheckForExistance(string address)
        {
            _address = address;
            GetCoordinatesFromAddress();
            return _existAddress;
        }

        async void GetCoordinatesFromAddress()
        {
            try
            {
                IEnumerable<Location> locations = await Geocoding.GetLocationsAsync(_address);
                Location loc = locations?.FirstOrDefault();
                if (loc != null)
                {
                    _location = loc;
                    _existAddress = true;
                }
            }
            catch (Exception) { }

            _existAddress = false;
        }

        async void GetAddressFromCoordinates()
        {
            try
            {
                IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(_location.Latitude, _location.Longitude);
                Placemark placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    _address =
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"Location:        {placemark.Location}\n";

                }
            }
            catch (Exception)
            {

            }
        }

        async void GetCityFromCoordinates()
        {
            try
            {
                IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(_location.Latitude, _location.Longitude);
                Placemark placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    _city = placemark.Locality;

                }
            }
            catch (Exception)
            {

            }
        }

    }
}
