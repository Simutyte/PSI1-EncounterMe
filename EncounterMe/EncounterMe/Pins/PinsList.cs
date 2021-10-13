// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EncounterMe;
using MvvmHelpers;
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

        public ObservableRangeCollection<MapPin> allObjects = new ObservableRangeCollection<MapPin>();

        private static readonly string s_filename = "pins.bin";

        //private CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();


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
                type = (ObjectType)type,
                styleType = (StyleType)style
            };

            newOne.GetCoordinatesFromAdress();

            list.Add(newOne);
            allObjects.Add(newOne);
            WriteAPinInFile(newOne);
        }

        public void AddPinByCoordinatesToList(string name, Location location, int type, int style, string details, WorkingHours hours, Image photo)
        {
            MapPin newOne = new MapPin(name)
            {
                location = location,
                description = details,
                hours = hours,
                image = photo,
                type = (ObjectType)type,
                styleType = (StyleType)style,
            };

            newOne.GetAddressFromCoordinates();

            list.Add(newOne);
            allObjects.Add(newOne);
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
            try
            {
                list = IO.ReadFromBinaryFile<List<MapPin>>(s_filename);

                foreach (MapPin pin in list)
                {
                    allObjects.Add(pin);
                }
            }
            catch(Exception) //null reference
            {

            }
        }
    }
}
