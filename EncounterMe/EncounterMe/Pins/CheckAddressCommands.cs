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
        private string _address { get; set; }
        private Location _location { get; set; }

        private bool _existAddress { get; set; }
        private string _city { get; set; }

        public CheckAddressCommands() { }

        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string GetAddress(Location location)
        {
            _location = location;
            //GetAddressFromCoordinates();
            return _address;
        }

        public Location GetCoordinates(string address)
        {
            _address = address;
            //GetCoordinatesFromAddress();
            return _location;
        }

        public string GetCity(Location location)
        {
            _location = location;
            //GetCityFromCoordinates();
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
            //try
            //{
            //    IEnumerable<Location> locations = await Geocoding.GetLocationsAsync(_address);
            //    Location loc = locations?.FirstOrDefault();
            //    if (loc != null)
            //    {
            //        _location = loc;
            //        _existAddress = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    _existAddress = false;
            //}

            var location = (await Geocoding.GetLocationsAsync($"{Street}, {City}, {Country}")).FirstOrDefault();

            if (location == null) return;
            _location = location;
        }

        public async void GetAddressFromCoordinates(Location location)
        {
            try
            {
                Console.WriteLine("0\n\n\n");
                //IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(_location.Latitude, _location.Longitude);
                //Placemark placemark = placemarks?.FirstOrDefault();

                var addrs = (await Geocoding.GetPlacemarksAsync(location)).FirstOrDefault();
                if (addrs != null)
                {
                    //_address =
                    //    $"CountryName:     {placemark.CountryName}\n" +
                    //    $"Location:        {placemark.Location}\n";

                    Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare}";
                    City = $"{addrs.PostalCode} {addrs.Locality}";
                    Country = addrs.CountryName;

                    Console.WriteLine(Street + " " + City + " " + Country);

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
               // await DisplayAlert("Exception", "Handle not supported on device exception", "OK");
                Console.WriteLine("1\n\n\n");
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
               // await DisplayAlert("Exception", "Handle not enabled on device exception", "OK");
                Console.WriteLine("2\n\n\n");
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // await DisplayAlert("Exception", "Handle permission exception", "OK");
                Console.WriteLine("3\n\n\n");
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // await DisplayAlert("Exception", "I fucking dont know", "OK");
                Console.WriteLine("4\n\n\n" + ex.Message.ToString());
                // Unable to get location
            }

            Console.WriteLine("5\n\n\n");

        }

        private Task DisplayAlert(string v1, string v2, string v3) => throw new NotImplementedException();

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
