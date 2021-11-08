// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Pins;

namespace EncounterMe.Services
{
    public class MapPinService
    {
        private PinsList _pinsList;
        public MapPinService()
        {
            PinsList PinsListTemp= PinsList.GetPinsList();
            _pinsList = PinsListTemp;
        }


        public async void TryToAdd(MapPin mapPin)
        {
            try
            {
                if(mapPin != null)
                {
                    await ApiMapPinService.AddMapPin(mapPin);
                    LoadList();
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        
        public async void TryToDelete(MapPin mapPin)
        {
            try
            {
                if (mapPin != null)
                {
                    await ApiMapPinService.DeleteMapPin(mapPin);
                    LoadList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //surašo gautus mapPin iš duomenų bazės į listOfPins
        public async void LoadList()
        {
            if (_pinsList.ListOfPins != null)
                _pinsList.ListOfPins.Clear();

            try
            {
                var mapPins = await ApiMapPinService.GetMapPins();
                if(mapPins != null)
                {
                    foreach(var mapPin in mapPins)
                    {
                       
                        _pinsList.ListOfPins.Add(mapPin);
                     
                    }
                    CorrectCoordinates();
                    UploadPins();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //Kadangi saugom duomenų bazėj latitude and longitude kaip double, čia bandau kiekvienam mapPin įrašyt tuos duomenis į location
        //Kadangi visur iš to location naudojam tik longitude ir latitude gal būtų lengviau ateity tiesiog tuos double naudot?
        public void CorrectCoordinates()
        {
            try
            {
                foreach(MapPin mapPin in _pinsList.ListOfPins)
                {
                    mapPin.CorrectLocation();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //kiekvienam mapPin bando sukurt po Pin, kadangi duomenų bazėj jo neina išsaugot
        public void UploadPins()
        {
            try
            {
                foreach (MapPin mapPin in _pinsList.ListOfPins)
                {
                    if(mapPin.Location != null)
                    {
                        mapPin.CreateAPin();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
