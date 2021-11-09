using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Models
{
    public class MapPin
    {
        public int Id { get; set; }

        public string Name { get; set; }

       
        public string Description { get; set; }

        public int AddressID { get; set; }

        public Address Address { get; set; }

        public string OpeningHours { get; set; }

        public string ClosingHours { get; set; }

        public String ImageName { get; set; }

        public ObjectType Type { get; set; }

        public StyleType StyleType { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

    }
}
