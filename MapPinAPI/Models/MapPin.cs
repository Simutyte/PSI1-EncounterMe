using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Models
{
    public class MapPin
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DefaultValue("About me")]
        public string Description { get; set; }

        public Address Address { get; set; }

        public string OpeningHours { get; set; }

        public string ClosingHours { get; set; }

        [DefaultValue("https://www.topdeal.lt/wp-content/themes/consultix/images/no-image-found-360x250.png")]
        public String ImageName { get; set; }

        public ObjectType Type { get; set; }

        public StyleType StyleType { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        //Navigation properties
        public  int CreatorId { get; set; }

        //public List<FavouriteMapPin> FavouritesMapPins { get; set; }

        public MapPin()
        {

        }

        public MapPin(string name, string description, Address address, string openingHours, string closingHours, string imageName,
                      ObjectType type, StyleType styleType, double longitude, double latitude )
        {
            Name = name;
            Description = description;
            Address = address;
            OpeningHours = openingHours;
            ClosingHours = closingHours;
            ImageName = imageName;
            Type = type;
            StyleType = styleType;
            Longitude = longitude;
            Latitude = latitude;
  

        }
    }
}
