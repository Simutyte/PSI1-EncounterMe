// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterMe.Pins
{
    [Serializable]
    public class WorkingHours
    {
        public int WorkingHoursID { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public WorkingHours()
        {

        }

        public WorkingHours(TimeSpan open, TimeSpan close)
        {
            OpeningTime = open;
            ClosingTime = close;
        }

       /* public WorkingHours(int id, TimeSpan open, TimeSpan close)
        {
            WorkingHoursID = id;
            OpeningTime = open;
            ClosingTime = close;
        }*/

    }
}
