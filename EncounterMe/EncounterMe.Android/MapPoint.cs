// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EncounterMe.Droid
{
    class Object
    {
        private string _name { get; set; }
        private string _adress { get; set; }
        public Xamarin.Forms.Maps.Position position { get; set; }
        public Xamarin.Forms.Maps.PinType pinType { get; set; }
        private string _description { get; set; }
        public TimeSpan openingHours { get; set; }
        public TimeSpan closingTime { get; set; }

        Object(string name, string adress, string description, Xamarin.Forms.Maps.Position position, TimeSpan open, TimeSpan close)
        {
            this._name = name;
            this._adress = adress;
            this._description = description;
            this.position = position;
            this.openingHours = open;
            this.closingTime = close;
        }


    }
}
