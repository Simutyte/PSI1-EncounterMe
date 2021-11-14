// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EncounterMe.Users
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(35), Unique]
        public string Username { get; set; }
        public string Password { get; set; }

        [MaxLength(100), Unique]
        public string Email { get; set; }

        //public List<Lazy<MapPin>> MyFavoriteObjects { get; set; }

        //public List<Lazy<MapPin>> MyVisitedObjects { get; set; }

        public User()
        {

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
