// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EncounterMe.Pins
{
    public class FavouritePin
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ObjectId { get; set; }

        public int UserId { get; set; }
    }
}
