// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Input;
using MvvmHelpers;

namespace EncounterMe.ViewModels
{
    class AllObjectsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<MapPin> allObjectsCollection { get; set; }

        public AllObjectsViewModel()
        {
            PinsList pinsList = PinsList.GetPinsList();
            allObjectsCollection = pinsList.allObjects;

        }
    }
}
