// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EncounterMe;
using Xamarin.Essentials;
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


        protected PinsList()
        {

        }

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


        public void AddPinByAddressToList(string name, string address, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                address = address,
                description = details,
                hours = hours,
                image = photo,
                type = (Type)type,
                styleType = (StyleType)style
            };

            newOne.GetCoordinatesFromAdress();

            list.Add(newOne);
        }

        public void AddPinByCoordinatesToList(string name, Location location, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                location = location,
                description = details,
                hours = hours,
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
            IO.WriteToXmlFile(objectToWrite: list, append: false, filePath: filename);
        }

        public void GetListOfPinsFromFile()
        {
            list = IO.ReadFromXmlFile<List<MapPin>>(filename);
        }
    }
}
