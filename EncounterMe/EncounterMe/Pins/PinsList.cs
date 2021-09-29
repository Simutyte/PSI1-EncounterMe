// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncounterMe
{
    class PinsList
    {

        static PinsList instance;
        private static object locker = new object();

        public List<MapPin> list = new List<MapPin>();

        private static string filename = "pins.xml";

        protected PinsList() { }

        public static PinsList GetPinsList()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new PinsList();
                    }
                }
            }
            return instance;
        }


        public void AddPinByAddressToList(string name, string address, int type, int style, string details, TimeSpan open, TimeSpan close, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                address = address,
                description = details,
                openingHours = open,
                closingTime = close,
                image = photo,
                type = (Type)type,
                styleType = (StyleType)style
            };

            newOne.GetCoordinatesFromAdress();

            list.Add(newOne);
        }

        public void AddPinByCoordinatesToList(string name, double lat, double lon, int type, int style, string details, TimeSpan open, TimeSpan close, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                latitude = lat,
                longitude = lon,
                description = details,
                openingHours = open,
                closingTime = close,
                image = photo,
                type = (Type)type,
                styleType = (StyleType)style,
            };

            newOne.GetAddressFromCoordinates();

            list.Add(newOne);
        }

        public void AddPinInMap(MapPin pin)
        {
            pin.CreateAPin();
            //cia map.Pins.Add(pin);
            //kur map to sukurto zemelapio pavadinimas kurio as nežinau dabar
        }

        public void WriteListOfPinsInFile()
        {
            IO.WriteToXmlFile(filename, list, false);
        }

        public void GetListOfPinsFromFile()
        {
            // error?!?
            // list = IO.ReadFromXmlFile(filename);
        }
    }
}
