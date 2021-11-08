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
    [Serializable]
    public class MapPin : IComparable
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public string Description { get; set; }

        public int AddressID { get; set; }

        public Address Address { get; set; }

        public int HoursID { get; set; }

        public WorkingHours Hours { get; set; }

        public String ImageName { get; set; }

        public ObjectType Type { get; set; }

        public StyleType StyleType { get; set; }

        //-------------------------------------
        [field: NonSerialized]
        public string StringAddress { get; set; }

        [field: NonSerialized]
        public Image Image { get; set; }

        [field: NonSerialized]
        public Location Location { get; set; }

        [field: NonSerialized]
        public Evaluation Evaluation { get; set; }

        [field: NonSerialized]
        public Pin Pin { get; set; }

        public MapPin()
        {

        }

        public MapPin( String name, string description, Address address, int type = 0, int styleType = 0,
                       WorkingHours hours = null, Location location = null, string image = "no image")
        {
            this.Name = name;
            this.Address = address;
            this.Hours = hours;
            this.Type = (ObjectType) type;
            this.StyleType =(StyleType) styleType;
            this.Description = description;
            this.ImageName = image;
            this.Location = location;
           

        }
        public MapPin(string name, Address address, WorkingHours hours = null, Location location = null,
                      ObjectType type = 0, StyleType styleType = 0, string description = "No description", Image image = null)
        {
            this.Name = name;
            this.Address = address;
            this.Location = location;
            this.Hours = hours;
            this.Type = type;
            this.StyleType = styleType;
            this.Description = description;
            this.Image = image;
            StringAddress = $"{address.Country} {address.City} {address.Street}";
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            MapPin otherMapPin = obj as MapPin;
            if (otherMapPin != null)
                return Name.CompareTo(otherMapPin.Name);
            else
                throw new ArgumentException("Object is not MapPin");
        }
    }
}
