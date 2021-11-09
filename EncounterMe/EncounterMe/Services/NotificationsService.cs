// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace EncounterMe.Services
{
    class NotificationsService
    {
        public void OnPinAdded(object source, AddedPinEventArgs args)
        {
            AppShell.Current.DisplayAlert("Notification", "Object named " + args.Pin.Name + " succesfully added", "Ok");
        }
    }
}
