// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using EncounterMe.Pins;

namespace EncounterMe
{
    class Route<MapPin>
    {
        public StyleType styleType { get; set; }

        public ObjectType objectType { get; set; }

        private readonly PinsList _objectsList = PinsList.GetPinsList();

        public List<EncounterMe.MapPin> route { get; set; }


        public void CreateARouteByStyleTypeAndCity(string city, int type)
        {
            styleType = (StyleType)type;

            foreach (EncounterMe.MapPin pin in _objectsList.listOfPins)
            {
                if (pin.styleType == styleType && pin.address.city == city)
                {
                    route.Add(pin);
                }
            }
        }

        public void CreateARouteByTypeAndCity(string city, int type)
        {
            objectType = (ObjectType)type;

            foreach (EncounterMe.MapPin pin in _objectsList.listOfPins)
            {
                if (pin.type == objectType && pin.address.city == city)
                {
                    route.Add(pin);
                }
            }
        }
    }
}
