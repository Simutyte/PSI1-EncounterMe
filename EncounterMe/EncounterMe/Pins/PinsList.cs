// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EncounterMe;
using EncounterMe.Pins;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncounterMe
{
    class PinsList
    {
        private static PinsList s_instance;

        private static readonly object s_locker = new object();

        public List<MapPin> list = new List<MapPin>();

        public ObservableRangeCollection<MapPin> allObjects = new ObservableRangeCollection<MapPin>();

        private static readonly string s_filename = "pins.xml";

        private CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();

        protected PinsList()
        {

        }

        public static PinsList GetPinsList()
        {
            if (s_instance == null)
            {
                lock (s_locker)
                {
                    if (s_instance == null)
                    {
                        s_instance = new PinsList();
                    }
                }
            }
            return s_instance;
        }


        public void AddPinByAddressToList(string name, string address, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name, address, _checkAddressCommands.GetCoordinates(address), hours,
                                      (ObjectType)type, (StyleType)style, details, photo);

            list.Add(newOne);
            allObjects.Add(newOne);
            //WriteListOfPinsInFile();
        }

        public void AddPinByCoordinatesToList(string name, Location location, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name, _checkAddressCommands.GetAddress(location), location, hours,
                                      (ObjectType)type, (StyleType)style, details, photo);

            list.Add(newOne);
            allObjects.Add(newOne);
        }

        public void AddPinInMap(MapPin pin)
        {
            pin.CreateAPin();
            //cia map.Pins.Add(pin);
            //kur map to sukurto zemelapio pavadinimas kurio as nežinau dabar
        }

        public void WriteListOfPinsInFile()
        {
            IO.WriteToXmlFile(objectToWrite: list, append: false, filePath: s_filename);
        }

        public void GetListOfPinsFromFile()
        {
            IO.ReadFromXmlFile<List<MapPin>>(s_filename);
        }
    }
}
