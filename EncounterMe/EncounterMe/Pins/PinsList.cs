// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using EncounterMe;
using EncounterMe.Pins;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace EncounterMe
{
    class PinsList
    {

        private static PinsList s_instance;

        private static readonly object s_locker = new object();

        public List<MapPin> ListOfPins = new List<MapPin>();

        private protected PinsList()
        {

        }

        public static PinsList GetPinsList()
        {
            if (s_instance == null)
            {
                lock (s_locker)
                {
                    if (s_instance == null)
                    {
                        s_instance = new PinsList();
                    }
                }
            }
            return s_instance;
        }
    }
}
