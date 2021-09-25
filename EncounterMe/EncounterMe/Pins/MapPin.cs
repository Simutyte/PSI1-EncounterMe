// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EncounterMe.Droid
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

        public double latitude { get; set; }
        public double longitude { get; set; }

        public Xamarin.Forms.Maps.Position position { get; set; }
        public Xamarin.Forms.Maps.PinType pinType { get; set; }

        public TimeSpan openingHours { get; set; }
        public TimeSpan closingTime { get; set; }

        public int type { get; set; }
        public int styleType { get; set; }

        public MapPin(string name)
        {
            this.name = name;
        }

        
        public async void GetCoordinatesFromAdress()
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    latitude = location.Latitude;
                    longitude = location.Longitude;
                }
            }
            catch (Exception ex) { }
        }

        public async void GetAdressFromCoordinates()
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    address =
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"Locality:        {placemark.Locality}\n";
                }
            }
            catch (Exception ex) { }
        }
     
    }
}
