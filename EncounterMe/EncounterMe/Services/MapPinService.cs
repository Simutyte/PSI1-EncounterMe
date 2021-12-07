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
        //Visus kintamuosius ir metodus iš čia naudot per App.s_mapPinService.kintamojoVardas / App.s_mapPinService.metodoVardas

        private PinsList _pinsList;

        public List<MapPin> ListOfPins;

        public List<MapPin> UserOwnerMapPins; // List'as objektų, kuriuos sukūrė dabartinis useris

        public List<MapPin> UserFavouriteMapPins; //List'as objektų, kuriuos pamėgo dabartinis useris
        public User CurrentUser { get; set; } //dabartinis useris
        public MapPinService()
        {
            UserOwnerMapPins = new List<MapPin>();
            UserFavouriteMapPins = new List<MapPin>();

            PinsList PinsListTemp= PinsList.GetPinsList();
            _pinsList = PinsListTemp;

            ListOfPins = new List<MapPin>();
        }

        public event PinAddedEventHandler<AddedPinEventArgs> PinAdded;

        //Eventai---------------------------------------------
        protected virtual void OnPinAdded(AddedPinEventArgs args)
        {
            if (PinAdded != null)
                PinAdded(this, args);
        }

        //-----------------------------------------------------

        //Metodas, kviečiamas kai bandom pridėt naują objektą
        public async void TryToAdd(MapPin mapPin)
        {
            try
            {
                if(mapPin != null)
                {
                    await ApiMapPinService.AddMapPin(mapPin);       //prideda į db atitinkamą lentelę
                    OnPinAdded(new AddedPinEventArgs(mapPin));      //kviečia eventą
                    LoadList();                                     //pasikeitė visų objektų list'as todėl jį reik užkraut per naują
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
            if (ListOfPins != null)
                ListOfPins.Clear();

            if (_pinsList.ListOfPins != null)
                _pinsList.ListOfPins.Clear();

            try
            {
                var mapPins = await ApiMapPinService.GetMapPins();      //gauna listą objektų iš db
                if(mapPins != null)
                {
                    foreach(var mapPin in mapPins)
                    {
                        _pinsList.ListOfPins.Add(mapPin);   //šitą siūlyčiau trint
                        ListOfPins.Add(mapPin);
                     
                    }
                    await UploadFavourites();                        //užkraunam patinkančius objektus per naują
                    UploadPins();                                       //bandom sukurt kiekvienam objektui po pin ant žemėlapio
                    LoadOwnerObjects();                                 //užkraunam objektus, kuriuos user'is yra sukūręs
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
                foreach (MapPin mapPin in ListOfPins)
                {
                    if(mapPin.Latitude != 0 && mapPin.Longitude != 0)
                    {
                        mapPin.CreateAPin();
                    }
                }

                //Šitą va siūlyčiau trint
                foreach (MapPin mapPin in _pinsList.ListOfPins)
                {
                    if (mapPin.Latitude != 0 && mapPin.Longitude != 0)
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

        //Kviečiamas, kai user'is paspaudžia širdutę AllObjectsPage
        public async void AddFavourite(MapPin mapPin)
        {
            //Sukuriamas ryšio objektas, nes db turim lentelę, kur saugom tiesiog user'io id ir objekto, kurį jis mėgstą id
            FavouriteMapPin userMapPin = new FavouriteMapPin()
            {
                UserId = CurrentUser.Id,
                MapPinId = mapPin.Id
            };

            await APIFavouriteMapPinService.AddFavouriteMapPin(userMapPin);     //pridedam į db atitinkamą lentelę
            await UploadFavourites();                                            //kadangi pridėjom naują favourite pin tai turim per naują užloadint
   
        }

        //Loadinam favourite objektus į sąrašą
        public async Task UploadFavourites()
        {
            //gaunam listą, kur būna visi dabartinio userio pamėgtų objektų id
            var UserFavouriteIds = await APIFavouriteMapPinService.GetFavouriteMapPins(CurrentUser.Id); 

            if (UserFavouriteMapPins != null)
                UserFavouriteMapPins.Clear();

            foreach(var favouriteMapPin in UserFavouriteIds)
            {
                if(ListOfPins != null)
                {
                    //kadangi gaunam tik objekto id, tai pagal tą id surandam sąraše atitinkamą objektą
                    var result = ListOfPins.Find(i => i.Id == favouriteMapPin.MapPinId);
                    if (result != null)
                        UserFavouriteMapPins.Add(result);
                }
            }
        }

        //Trinam patinkantį objektą. Ištrinam tik iš ryšio lentelės, bet ne patį objektą
        public async void DeleteFavourite(MapPin mapPin)
        {
            await APIFavouriteMapPinService.DeleteFavouriteMapPin(CurrentUser.Id, mapPin.Id); //trinam iš db lentelės FavouriteMapPins
            await UploadFavourites();                                                        //pasikeitė pamėgtų sąrašas, todėl load'inam jį per naują
        }


        //Load'inam objektus, kuriuos dabartinis vartotas yra sukuręs ir sudedam juos į list'ą
        public void LoadOwnerObjects()
        {
            if (UserOwnerMapPins != null)
                UserOwnerMapPins.Clear();

            try
            { 
                if (ListOfPins != null)
                {
                    foreach (var mapPin in ListOfPins)
                    {
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

        //Pakeitėm kažkokią info user'į - jį updatinam
        public async Task UpdatingUser(User user)
        {
            await ApiUserService.UpdateUser(user);      //updatinam db
            await GetCurrentUserAsync(user.Id);         //kviečiam jog per naują išsaugotų dabartinį user'į, jog turėtumėm jį su atnaujinta info
        }

        //Gaunam dabartinį user'į
        public async Task GetCurrentUserAsync(int id)
        {
            CurrentUser = await ApiUserService.GetUser(id); //gaunam jį iš db
            LoadOwnerObjects();                            
            await UploadFavourites();
           
        }

        //Ištrynimas user'io objekto
        public async Task DeleteOwnedObjects(MapPin mapPin)
        {
            await ApiMapPinService.DeleteMapPin(mapPin);    //ištrinam iš db
            LoadList();                                     //per naują perrašom list'ą visų objektų, nes jis pasikeitė
        }

        //Prisijungimo validacija
        public async Task<User> LogInValidate(string username, string pass)
        {
            var allUsers = await ApiUserService.GetUsers();     //gaunam visus userius

            foreach(var u in allUsers)
            {
                if(string.Equals(u.Username, username) && string.Equals(u.Password, pass))      //ieškom tokio, kur sutaptų username ir pass
                {
                    return u;
                }
            }
            return null;
        }
    }
}
