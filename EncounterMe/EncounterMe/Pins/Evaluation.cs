// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncounterMe.Pins
{
    [Serializable]
    public class Evaluation
    {
        public int MapPinId { get; set; }

        public int UserId { get; set; }

        public int Value { get; set; }

        public Evaluation()
        {

        }

        public Evaluation(int mapPinId, int userId, int value)
        {
            MapPinId = mapPinId;
            UserId = userId;
            Value = value;
        }
    }
}
