// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Pins;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace EncounterMe.Users
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(35), Unique]
        public string Username { get; set; }
        public string Password { get; set; }

        [MaxLength(100), Unique]
        public string Email { get; set; }

        public bool HasFavourite { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<FavouritePin> FavouriteObjects { get; set; }
        //public Lazy<List<FavouritePin>> FavouriteObjects { get; set; }

        //public List<Lazy<MapPin>> MyFavoriteObjects { get; set; }

        //public List<Lazy<MapPin>> MyVisitedObjects { get; set; }

        public User()
        {
            HasFavourite = false;
            FavouriteObjects = new List<FavouritePin>();
            //FavouriteObjects = new Lazy<List<FavouritePin>>(() => new List<FavouritePin>());
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
        //public void SetMyFavoriteObjects(MapPin pin)
        //{
        //    MyFavoriteObjects.Add(new Lazy<MapPin>(() => pin));
        //}

        //public void SetMyVisitedObjects(MapPin pin)
        //{
        //    MyVisitedObjects.Add(new Lazy<MapPin>(() => pin));
        //}
    }
}
