// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EncounterMe.Pins
{
    public class Calculating
    {
      
        public static double GetDistanceInKm(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju km");
            Console.WriteLine(Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Kilometers));
            return Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Kilometers);

        }

        public static double GetDistanceInMiles(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju myliom");
            Console.WriteLine(Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Miles));
            return Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Miles);

        }

        public static double GetDistanceInM(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju metrais");
            Console.WriteLine(GetDistanceInKm(location, pin) * 1000);
            return GetDistanceInKm(location, pin) * 1000;

        }

        public static double GetDistanceInYards(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju yardais");
            Console.WriteLine(GetDistanceInMiles(location, pin) * 1.760);
            return GetDistanceInMiles(location, pin) * 1.760;

        }
    }
}
