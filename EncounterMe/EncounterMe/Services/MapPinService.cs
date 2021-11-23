// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Pins;

namespace EncounterMe.Services
{
    public delegate void PinAddedEventHandler<T>(object source, T args);

    public class MapPinService
    {
        private PinsList _pinsList;
        public MapPinService()
        {
            PinsList PinsListTemp= PinsList.GetPinsList();
            _pinsList = PinsListTemp;
        }

        public event PinAddedEventHandler<AddedPinEventArgs> PinAdded;
        public event PinAddedEventHandler<EventArgs> RefreshList;

        protected virtual void OnPinAdded(AddedPinEventArgs args)
        {
            if (PinAdded != null)
                PinAdded(this, args);
        }

        protected virtual void OnRefreshList(EventArgs args)
        {
            if (RefreshList != null)
                RefreshList(this, args);
        }


        public async void TryToAdd(MapPin mapPin)
        {
            try
            {
                if(mapPin != null)
                {
                    await ApiMapPinService.AddMapPin(mapPin);
                    OnPinAdded(new AddedPinEventArgs(mapPin));
                    OnRefreshList(EventArgs.Empty);
                    LoadList();
                }
                
            }
            catch(Exception ex)
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
                    UploadPins();
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
                    if(mapPin.Latitude != 0 && mapPin.Longitude != 0)
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
