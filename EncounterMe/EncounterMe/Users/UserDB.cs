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


        public List<FavouritePin> GetAllFavPins()
        {
            var data = _mySQLiteConnection.Table<FavouritePin>();
            return  data.ToList();
        }

        public bool AddFavPin(MapPin pin)
        {
            var data = _mySQLiteConnection.Table<FavouritePin>();
            var pinToAdd = GetFavouritePin(pin);

            var d1 = data.Where(x => x.UserId == pinToAdd.UserId && x.ObjectId == pinToAdd.ObjectId).FirstOrDefault();
            if (d1 == null)
            {
                Console.WriteLine("pateko i vidu");
                var user = GetUserByID((int)CurrentUserId);
                user.HasFavourite = true;
                _mySQLiteConnection.Update(user);
                

                _mySQLiteConnection.Insert(pinToAdd);
                return true;
            }
            else
                return false;
        }

        public FavouritePin GetFavouritePin(MapPin mapPin)
        {
            FavouritePin favouritePin = new FavouritePin()
            {
                ObjectId = mapPin.Id,
                UserId = (int)App.s_userDb.CurrentUserId
            };

            return favouritePin;

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
                
                return true;
            }
            else
                return false;
        }
    }
}
