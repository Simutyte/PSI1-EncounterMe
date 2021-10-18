// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EncounterMe.Pins
{
    class CheckAddressCommands
    {
        private Location _location { get; set; }

        private bool _existAddress { get; set; }

        public string street { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string postalCode { get; set; }

        public CheckAddressCommands()
        {

        }

        public Location GetCoordinates(string xcountry, string xcity, string xpostal, string xstreet)
        {
            country = xcountry;
            city = xcity;
            postalCode = xpostal;
            street = xstreet;
            GetCoordinatesFromAddress();
            return _location;
        }

        public string GetCity(Location location)
        {
            _location = location;
            //GetCityFromCoordinates();
            return city;
        }

        public bool CheckForExistance(string xcountry, string xcity, string xpostal, string xstreet)
        {
            country = xcountry;
            city = xcity;
            postalCode = xpostal;
            street = xstreet;
            GetCoordinatesFromAddress();
            return _existAddress;
        }

        async void GetCoordinatesFromAddress()
        {
            var location = (await Geocoding.GetLocationsAsync($"{street}, {city}, {postalCode}, {country}")).FirstOrDefault();

            if (location == null)
            {
                _existAddress = false;
                return;
            }
            _existAddress = true;
            _location = location;
        }

        public async 
        Task
        GetAddressFromCoordinates(Location location)
        {
            try
            {
                var addrs = (await Geocoding.GetPlacemarksAsync(location)).FirstOrDefault();
                if (addrs != null)
                {
                    street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare}";
                    postalCode = $"{addrs.PostalCode}";
                    city = $"{addrs.Locality}";
                    country = addrs.CountryName;
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                // Handle permission exception
            }
            catch (Exception)
            {
                // Unable to get location
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
                    city = placemark.Locality;

                }
            }
            catch (Exception)
            {

            }
        }

    }
}
