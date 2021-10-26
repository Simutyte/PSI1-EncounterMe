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

        public Address Address;

        public CheckAddressCommands()
        {

        }

        public Location GetCoordinates(string xcountry, string xcity, string xpostal, string xstreet)
        {
            Address = new Address(xcountry, xcity, xstreet, xpostal);
            GetCoordinatesFromAddress();
            return _location;
        }

        public bool CheckForExistance(string xcountry, string xcity, string xpostal, string xstreet)
        {
            Address = new Address(xcountry, xcity, xstreet, xpostal);
            GetCoordinatesFromAddress();
            return _existAddress;
        }

        async void GetCoordinatesFromAddress()
        {
            var location = (await Geocoding.GetLocationsAsync($"{Address.Street}, {Address.City}, {Address.PostalCode}, {Address.Country}")).FirstOrDefault();

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
                    Address.Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare}";
                    Address.PostalCode = $"{addrs.PostalCode}";
                    Address.City = $"{addrs.Locality}";
                    Address.Country = addrs.CountryName;
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                throw;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                throw;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                throw;
            }
            catch (Exception)
            {
                // Unable to get location
            }
        }
    }
}
