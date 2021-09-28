// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Droid;
using Type = EncounterMe.Droid.Type;

namespace EncounterMe.Pins
{
    class Route<MapPin>
    {
        public StyleType styleType { get; set; }
        public System.Type type { get; set; }

        public List<MapPin> route { get; set; }

        public void createARouteByStyleTypeAndCity( string city, int type)
        {
            StyleType styleType = (StyleType)type;

            //find all objects from PinsList
            
        }
        public void createARouteByTypeAndCity(string city, int type)
        {
            Type styleType = (Type)type;

            //find all objects from PinsList

        }

        public void createACustomRoute()
        {
            //no idea yet
        }



    }
}
