// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using EncounterMe.Pins;

namespace EncounterMe
{
    class Route<MapPin>
    {
        public StyleType StyleType { get; set; }

        public ObjectType ObjectType { get; set; }

        //private readonly PinsList _objectsList = PinsList.GetPinsList();

        private readonly PinsList _objectsList = PinsList.Instance;

        public List<EncounterMe.MapPin> RouteOfObjects { get; set; }


        public void CreateARouteByStyleTypeAndCity(string city, int type)
        {
            StyleType = (StyleType)type;

            foreach (EncounterMe.MapPin pin in _objectsList.ListOfPins)
            {
                if (pin.StyleType == StyleType && pin.Address.City == city)
                {
                    RouteOfObjects.Add(pin);
                }
            }
        }

        public void CreateARouteByTypeAndCity(string city, int type)
        {
            ObjectType = (ObjectType)type;

            foreach (EncounterMe.MapPin pin in _objectsList.ListOfPins)
            {
                if (pin.Type == ObjectType && pin.Address.City == city)
                {
                    RouteOfObjects.Add(pin);
                }
            }
        }
    }
}
