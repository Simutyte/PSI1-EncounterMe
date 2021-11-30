// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Pins;
using EncounterMe.Users;

namespace EncounterMe.Services
{
    public delegate void PinAddedEventHandler<T>(object source, T args);

    public class MapPinService
    {
        private PinsList _pinsList;

        public List<MapPin> UserOwnerMapPins;
        public User CurrentUser { get; set; }
        public MapPinService()
        {
            UserOwnerMapPins = new List<MapPin>();
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

        public void LoadOwnerObjects()
        {
            //Console.WriteLine(CurrentUser.Id +" "+ CurrentUser.Username);
            if(CurrentUser == null)
                Console.WriteLine("Useris null");
            if (UserOwnerMapPins != null)
                UserOwnerMapPins.Clear();

            try
            {
                
                if (_pinsList.ListOfPins != null)
                {
                    foreach (var mapPin in _pinsList.ListOfPins)
                    {
                        Console.WriteLine(mapPin.CreatorId + "   " + CurrentUser.Id);
                        if (mapPin.CreatorId == CurrentUser.Id)
                        {
                            UserOwnerMapPins.Add(mapPin);

                        }
                            

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task GetCurrentUserAsync(int id)
        {
            CurrentUser = await ApiUserService.GetUser(id);
           
        }

        public async Task<User> LogInValidate(string username, string pass)
        {
            var allUsers = await ApiUserService.GetUsers();

            foreach(var u in allUsers)
            {
                if(string.Equals(u.Username, username) && string.Equals(u.Password, pass))
                {
                    return u;
                }
            }
            return null;
        }
    }
}
