﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace EncounterMe
{
    class Route<MapPin>
    {
        public StyleType styleType { get; set; }

        public ObjectType type { get; set; }

        PinsList objectsList = PinsList.GetPinsList();

        public List<EncounterMe.MapPin> route { get; set; }


        public void CreateARouteByStyleTypeAndCity(string city, int type)
        {
            StyleType styleType = (StyleType)type;

            for(int i=0; i<objectsList.list.Count; i++)
            {
                if(objectsList.list[i].styleType == styleType && objectsList.list[i].city == city)
                {
                    route.Add(objectsList.list[i]);
                }
            }     
        }

        public void CreateARouteByTypeAndCity(string city, int type)
        {
            ObjectType objectType = (ObjectType)type;

            for (int i = 0; i < objectsList.list.Count; i++)
            {
                if (objectsList.list[i].type == objectType && objectsList.list[i].city == city)
                {
                    route.Add(objectsList.list[i]);
                }
            }

        }

        public void CreateACustomRoute()
        {
            //no idea yet
        }


    }
}
