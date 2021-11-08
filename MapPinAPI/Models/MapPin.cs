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

        public int HoursID { get; set; }

        public WorkingHours Hours { get; set; }

        public String ImageName { get; set; }

        public ObjectType Type { get; set; }

        public StyleType StyleType { get; set; }
    }
}
