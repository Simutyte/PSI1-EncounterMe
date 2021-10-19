// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using EncounterMe.Pins;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EncounterMe
{
    class PinsList
    {
        private static PinsList s_instance;

        private static readonly object s_locker = new object();

        public List<MapPin> list = new List<MapPin>();

        private static readonly string s_filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "pins.bin");

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


        public void AddPinByAddressToList(string name, Address address, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name, address, null, hours,
                                      (ObjectType)type, (StyleType)style, details, photo);

            list.Add(newOne);
            WriteAPinInFile(newOne);
        }

        public void AddPinByCoordinatesToList(string name, Address address, Location location, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name, address, location, hours,
                                      (ObjectType)type, (StyleType)style, details, photo);

            list.Add(newOne);
            WriteAPinInFile(newOne);
        }

        public void AddPinInMap(MapPin pin)
        {
            pin.CreateAPin();
            //cia map.Pins.Add(pin);
            //kur map to sukurto zemelapio pavadinimas kurio as nežinau dabar
        }

        public void WriteAPinInFile(MapPin pin)
        {
            IO.WriteToBinaryFile<MapPin>(append: true, filePath: s_filePath, objectToWrite: pin);
        }

        public void GetListOfPinsFromFile()
        {
            list = IO.ReadFromBinaryFile<List<MapPin>>(s_filePath);
        }
    }
}
