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
            double dist = Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Kilometers);
            Console.WriteLine(dist.ToString());
            return dist;

        }

        public static double GetDistanceInMiles(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju myliom");
            double dist = Location.CalculateDistance(location.Latitude, location.Longitude, pin.Latitude, pin.Longitude, DistanceUnits.Miles);
            Console.WriteLine(dist.ToString());
            return dist;

        }

        public static double GetDistanceInMeters(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju metrais");
            double dist = GetDistanceInKm(location, pin) * 1000;
            Console.WriteLine(dist.ToString());
            return dist;

        }

        public static double GetDistanceInYards(Location location, MapPin pin)
        {
            Console.WriteLine("skaiciuoju yardais");
            double dist = GetDistanceInMiles(location, pin) * 1760;
            Console.WriteLine(dist.ToString());
            return dist;

        }
    }
}
