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
        public Location Location = new Location();
        public Address Address = new Address();

        private bool _existAddress { get; set; }

        public CheckAddressCommands()
        {

        }

        public async
        Task
        GetCoordinatesFromAddress(Address address)
        {
            try
            {
                var location = (await Geocoding.GetLocationsAsync($"{address.Street}, {address.City}, {address.PostalCode}, {address.Country}")).FirstOrDefault();
                if (location == null)
                {
                    _existAddress = false;
                    return;
                }
                _existAddress = true;
                Location = location;
            }
            catch(Exception)
            {
                Console.WriteLine("Koordinaciu gauti nepavyko");
            }
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
