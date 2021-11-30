// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }       

        public string PhotoPath { get; set; }

        [DefaultValue("About me")]
        public string AboutMe { get; set; }

        [DefaultValue(0)]
        public int Score { get; set; }

        [DefaultValue(false)]
        public bool HasFavourite { get; set; }

        //public List<MapPin> Favourites { get; set; }
        //public virtual List<MapPin> Favourites { get; set; }

        //Navigation properties


    }
}
