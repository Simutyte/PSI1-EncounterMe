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

        public Address Address { get; set; }

        public int HoursID { get; set; }

        public WorkingHours Hours { get; set; }

        public String ImageName { get; set; }

        public ObjectType Type { get; set; }

        public StyleType StyleType { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        //-------------------------------------


        public Image Image { get; set; }

        public Evaluation Evaluation { get; set; }
           
        public Pin Pin { get; set; }

        public MapPin()
        {

        }

        public MapPin( String name, string description, Address address, int type = 0, int styleType = 0,
                       WorkingHours hours = null, double latitude = 0, double longitude = 0, string image = "no image")
        {
            Name = name;
            Address = address;
            Hours = hours;
            Type = (ObjectType) type;
            StyleType =(StyleType) styleType;
            Description = description;
            ImageName = image;
            Longitude = longitude;
            Latitude = latitude;

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
