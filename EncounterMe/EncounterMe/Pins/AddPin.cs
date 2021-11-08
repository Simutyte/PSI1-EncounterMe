// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace EncounterMe.Pins
{
    public static class AddPin
    {
        
        public static void CreateAPin(this MapPin mapPin)
        {
            if(CheckLatitude(mapPin.Latitude) && CheckLongitude(mapPin.Longitude))
            {
                mapPin.Pin = new Pin()
                {
                    Label = mapPin.Name,
                    Address = mapPin.Address.Country + " " + mapPin.Address.City + " " + mapPin.Address.Street + " " + mapPin.Address.PostalCode,
                    Type = PinType.Place,
                    Position = new Position(mapPin.Latitude, mapPin.Longitude)
                };
            }
            
        }

        private static bool CheckLatitude(double lat)
        {
            if(lat <= 90 && lat >= -90)
            {
                return true;
            }
            return false;
        }

        private static bool CheckLongitude(double longit)
        {
            if (longit <= 180 && longit >= -180)
            {
                return true;
            }
            return false;
        }
    }
}
