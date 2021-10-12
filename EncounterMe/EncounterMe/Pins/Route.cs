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

        private CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();


        public void CreateARouteByStyleTypeAndCity(string city, int type)
        {
            styleType = (StyleType)type;

            for (int i = 0; i < _objectsList.list.Count; i++)
            {
                if (_objectsList.list[i].styleType == styleType &&
                    _checkAddressCommands.GetCity(_objectsList.list[i].location) == city)
                {
                    route.Add(_objectsList.list[i]);
                }
            }
        }

        public void CreateARouteByTypeAndCity(string city, int type)
        {
            objectType = (ObjectType)type;

            for (int i = 0; i < _objectsList.list.Count; i++)
            {
                if (_objectsList.list[i].type == objectType &&
                    _checkAddressCommands.GetCity(_objectsList.list[i].location) == city)
                {
                    route.Add(_objectsList.list[i]);
                }
            }

        }

        public void CreateACustomRoute()
        {
            //no idea yet
        }


    }
}
