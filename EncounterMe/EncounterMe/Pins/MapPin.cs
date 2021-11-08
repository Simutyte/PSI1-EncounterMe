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

        public double Longitude { get; set; }

        public double Latitude { get; set; }

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
            Name = name;
            Address = address;
            Hours = hours;
            Type = (ObjectType) type;
            StyleType =(StyleType) styleType;
            Description = description;
            ImageName = image;
            Location = location;
            Longitude = location.Longitude;
            Latitude = location.Latitude;

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

        public void CorrectLocation()
        {
            Location = new Location();
            Location.Latitude = Latitude;
            Location.Longitude = Longitude;
        }
    }
}
