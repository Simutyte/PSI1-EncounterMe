// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using EncounterMe.Pins;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace EncounterMe
{
    public class MapPin : IComparable
    {
        public string name { get; set; }

        public Address address { get; set; }

        public string stringAddress { get; set; }

        public string description { get; set; }

        public Image image { get; set; }

        public Location location { get; set; }

        public WorkingHours hours { get; set; }

        public ObjectType type { get; set; }

        public StyleType styleType { get; set; }

        public Pin pin { get; set; }

        public MapPin(string name, Address address = new Address(), Location location = null, WorkingHours hours = new WorkingHours(),
                      ObjectType type = 0, StyleType styleType = 0, string description = "No description", Image image = null)
        {
            this.name = name;
            this.address = address;
            this.location = location;
            this.hours = hours;
            this.type = type;
            this.styleType = styleType;
            this.description = description;
            this.image = image;
            this.stringAddress = this.address.country + " " + this.address.city + " " + this.address.street;
        }

        public int CompareTo(Object obj)
        {
            if (obj == null)
                return 1;

            MapPin otherMapPin = obj as MapPin;
            if (otherMapPin != null)
                return this.name.CompareTo(otherMapPin.name);
            else
                throw new ArgumentException("Object is not MapPin");
        }
    }
}
