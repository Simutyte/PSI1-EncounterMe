// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncounterMe
{

    [Flags]
    public enum StyleType
    {
        None = 0,
        Gothic = 1,
        Renaissance = 2,
        Baroque = 3,
        Classicism = 4
    }

    public enum Type
    {
        None = 0,
        Church = 1,
        CognitiveTrails = 2,
        Entertainment = 3,
        Manor = 4,
        Monument = 5,
        Mound = 6,
        Museum = 7,
        Park = 8,
        ReviewLocations = 9
    }

    class MapPin
    {
        public string name { get; set; }

        public string address { get; set; }

        public string description { get; set; }

        public Image image{ get; set; }

        public Location location { get; set; }

        public TimeSpan openingHours { get; set; }

        public TimeSpan closingTime { get; set; }

        public Type type { get; set; }

        public StyleType styleType { get; set; }

        public Pin pin { get; set; }

        public string city { get; set; }


        public MapPin(string name)
        {
            this.name = name;
        }

        
        public async void GetCoordinatesFromAdress()
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);
                var loc = locations?.FirstOrDefault();
                if (loc != null)
                {
                    location = loc;
                }
            }
            catch (Exception ex) { }
        }

        public async void GetAddressFromCoordinates()
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    address =
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"Location:        {placemark.Location}\n";

                }
            }
            catch (Exception ex)
            {

            }
        }

        //reikia kad galėčiau sudarinėti kelius pagal miestą
        public async void GetCity()
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    city = placemark.Locality;

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CreateAPin()
        {
            pin = new Pin
            {
                Label = name,
                Address = address,
                Type = PinType.Place,
                Position = new Position(location.Latitude, location.Longitude)
            };
        }
     
    }
}
