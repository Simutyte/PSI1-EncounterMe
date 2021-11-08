// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

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
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
