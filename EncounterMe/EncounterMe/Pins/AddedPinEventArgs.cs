// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace EncounterMe
{
    public class AddedPinEventArgs : EventArgs
    {
        public MapPin Pin { get; set; }

        public AddedPinEventArgs(MapPin pin)
        {
            Pin = pin;
        }

        public AddedPinEventArgs()
        {

        }
    }
}
