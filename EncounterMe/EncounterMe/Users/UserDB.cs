// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System;
using EncounterMe.Pins;
using System.Linq;
using SQLiteNetExtensions.Extensions;

namespace EncounterMe.Users
{
    public class UserDB
    {
        private SQLiteConnection _mySQLiteConnection;
        public int? CurrentUserId { get; set; }
        public UserDB()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            _mySQLiteConnection = new SQLiteConnection(dbPath);
            _mySQLiteConnection.CreateTable<User>();
            _mySQLiteConnection.CreateTable<FavouritePin>();
            CurrentUserId = -1;
        }

        public User GetUserByID(int id)
        {
            return _mySQLiteConnection.Table<User>().Where(i => i.ID == id).FirstOrDefault();
        }

        public User GetUserWithChildren(int id)
        {
            return _mySQLiteConnection.GetAllWithChildren<User>().Where(i => i.ID == id).FirstOrDefault();
        }

        public void DeleteUser(int id)
        {
            _mySQLiteConnection.Delete<User>(id);
        }

        public bool UpdateUser(int id, MapPin pin)
        {
            
            var d1 = GetUserWithChildren(id);

            if (d1 != null)
            {
                d1.HasFavourite = true;

                var favPin = d1.GetFavouritePin(pin);
                _mySQLiteConnection.Insert(favPin);

                //d1.FavouriteObjects.Value.Add(favPin);
                d1.FavouriteObjects.Add(favPin);
                
                if (d1.FavouriteObjects.Count == 0)
                {
                    AppShell.Current.DisplayAlert("buvo 0", "buvo 0", "ok");
                    //Console.WriteLine("BUVO 0!!!!!!!!!!!!!!");
                }
                else
                {
                    AppShell.Current.DisplayAlert("nebuvo 0", "buvo "+ d1.FavouriteObjects.Count, "ok");
                }


                
                _mySQLiteConnection.UpdateWithChildren(d1);
                
                return true;
            }
            return false;

        }

        public bool AddUser(User user)
        {
            var data = _mySQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.Username == user.Username || x.Email == user.Email).FirstOrDefault();
            if (d1 == null)
            {
                _mySQLiteConnection.Insert(user);
                return true;
            }
            else
                return false;
        }

        public bool LoginValidate(string username, string pass)
        {
            var data = _mySQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.Username == username && x.Password == pass).FirstOrDefault();

            if (d1 != null)
            {
                CurrentUserId = d1.ID;
                App.s_mapPinService.LoadFavourites(App.s_userDb.GetUserWithChildren((int)App.s_userDb.CurrentUserId));
                return true;
            }
            else
                return false;
        }
    }
}
