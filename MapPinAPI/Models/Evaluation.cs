// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapPinAPI.Models
{
    public class Evaluation
    {
        public int UserId { get; set; }
        public int MapPinId { get; set; }

        public int Value { get; set; }

        public Evaluation()
        {

        }

        public Evaluation( int userId, int mapPinId, int value)
        {
            MapPinId = mapPinId;
            UserId = userId;
            Value = value;
        }
    }
}
