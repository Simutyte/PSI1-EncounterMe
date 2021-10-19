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
            mapPin.pin = new Pin()
            {
                Label = mapPin.name,
                Address = mapPin.address.country + mapPin.address.city + mapPin.address.postalCode + mapPin.address.street,
                Type = PinType.Place,
                Position = new Position(mapPin.location.Latitude, mapPin.location.Longitude)
            };
        }
    }
}
