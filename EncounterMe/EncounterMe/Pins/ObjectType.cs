// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterMe.Pins
{
    [Flags]
    public enum ObjectType
    {
        None = 0,
        Church = 1,
        CognitiveTrails = 2,
        Entertainment = 3,
        Manor = 4,
        Monument = 5,
        Mound = 6,
        Museum = 7,
        Park = 8,
        ReviewLocations = 9
    }
}
