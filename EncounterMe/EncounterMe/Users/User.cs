// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

namespace EncounterMe.Users
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhotoPath { get; set; }

        public string AboutMe { get; set; }

        public int Score { get; set; }

        //public List<MapPin> Favourites { get; set; }
        //public virtual List<MapPin> Favourites { get; set; }

  

        //public List<Lazy<MapPin>> MyFavoriteObjects { get; set; }

        //public List<Lazy<MapPin>> MyVisitedObjects { get; set; }

        public User()
        {
            AboutMe = "Info about me";
            
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
