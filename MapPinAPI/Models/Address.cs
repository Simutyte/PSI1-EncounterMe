using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }

        public Address(string country, string city, string postalCode, string street)
        {
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
        }

        public Address()
        {
        }
    }
}
