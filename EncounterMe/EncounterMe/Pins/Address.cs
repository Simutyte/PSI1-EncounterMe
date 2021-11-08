// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterMe.Pins
{
    [Serializable]
    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }

        public Address(string country, string city, string code, string street)
        {
            this.Country = country;
            this.City = city;
            this.PostalCode = code;
            this.Street = street;
        }

        public Address()
        {

        }
    }
}
