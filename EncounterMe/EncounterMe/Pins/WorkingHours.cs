// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterMe.Pins
{
    public struct WorkingHours
    {
        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public WorkingHours(TimeSpan open, TimeSpan close)
        {
            OpeningTime = open;
            ClosingTime = close;
        }
    }
}
