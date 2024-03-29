﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterMe.Pins
{
    [Flags]
    [Serializable]
    public enum StyleType
    {
        None = 0,
        Gothic = 1,
        Renaissance = 2,
        Baroque = 3,
        Classicism = 4
    }
}
