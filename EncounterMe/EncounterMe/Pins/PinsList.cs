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




//var status = await Permissions.RequestAsync<Permissions.StorageRead>();
//var status = await Permissions.RequestAsync<Permissions.StorageWrite>();

//Console.WriteLine("")

namespace EncounterMe
{
    class PinsList
    {
        private static PinsList s_instance;

        private static readonly object s_locker = new object();

        public List<MapPin> listOfPins = new List<MapPin>();

        private static readonly string s_filename = "pins.bin";

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

            listOfPins.Add(newOne);
            WriteAPinInFile(newOne);
        }

        public void AddPinByCoordinatesToList(string name, Address address, Location location, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name, address, location, hours,
                                      (ObjectType)type, (StyleType)style, details, photo);

            listOfPins.Add(newOne);
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
            IO.WriteToBinaryFile<MapPin>(append: true, filePath: s_filename, objectToWrite: pin);
        }

        public void GetListOfPinsFromFile()
        {
            listOfPins = IO.ReadFromBinaryFile<List<MapPin>>(s_filePath);
        }
    }
}
