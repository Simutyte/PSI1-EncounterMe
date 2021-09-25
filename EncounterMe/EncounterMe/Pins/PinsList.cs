// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncounterMe.Pins
{
    class PinsList
    {

        public List<MapPin> list = new List<MapPin>();

        private static string filename = "pins.xml";

        public void addPinByAddressToList(string name, string address, int type, int style, string details, TimeSpan open, TimeSpan close, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                address = address,
                description = details,
                openingHours = open,
                closingTime = close,
                image = photo,
                type = type,
                styleType = style
            };

            newOne.GetCoordinatesFromAdress();

            list.Add(newOne);
        }

        public void addPinByCoordinatesToList(string name, double lat, double lon, int type, int style, string details, TimeSpan open, TimeSpan close, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                latitude = lat,
                longitude = lon,
                description = details,
                openingHours = open,
                closingTime = close,
                image = photo,
                type = type,
                styleType = style
            };

            newOne.GetAdressFromCoordinates();

            list.Add(newOne);
        }

        public void addPinInMap(MapPin pin)
        {
            Pin mapPin = new Pin
            {
                Label = pin.name,
                Address = pin.address,
                Type = PinType.Place,
                Position = new Position(pin.latitude , pin.longitude)
            };

            //cia map.Pins.Add(pin);
            //kur map to sukurto zemelapio pavadinimas kurio as nežinau
        }

        public void writeListOfPinsInFile()
        {
            for( int i=0; i<list.Count; i++)
            {
                IO.WriteToXmlFile(filename, list[i], false);
            }
        }
    }
}
