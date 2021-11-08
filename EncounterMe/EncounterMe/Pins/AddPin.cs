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
            mapPin.Pin = new Pin()
            {
                Label = mapPin.Name,
                Address = mapPin.Address.Country + " " + mapPin.Address.City + " " + mapPin.Address.Street + " " + mapPin.Address.PostalCode,
                Type = PinType.Place,
                Position = new Position(mapPin.Location.Latitude, mapPin.Location.Longitude)
            };
        }
    }
}
