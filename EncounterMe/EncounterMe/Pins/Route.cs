// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using EncounterMe.Pins;

namespace EncounterMe
{
    public class Route
    {
        public string City { get; set; }
        public StyleType Style { get; set; }
        public IEnumerable<MapPin> MapPins { get; set; }
        public int Count { get; set; }
    }
}
