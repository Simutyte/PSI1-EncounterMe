// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmHelpers;

namespace EncounterMe.ViewModels
{
    class AddObjectByAddressViewModel : BaseViewModel
    {
        private StyleType _selectedStyleType;

        public StyleType selectedStyleType
        {
            get
            {
                return _selectedStyleType;
            }
            set
            {
                SetProperty(ref _selectedStyleType, value);
            }
        }

        public List<String> styleTypeNames
        {
            get
            {
                return Enum.GetNames(typeof(StyleType)).Select(b => b).ToList();
            }
        }

        private ObjectType _selectedObjectType;

        public ObjectType selectedObjectType
        {
            get
            {
                return _selectedObjectType;
            }
            set
            {
                SetProperty(ref _selectedObjectType, value);
            }
        }

        public List<String> objectTypeNames
        {
            get
            {
                return Enum.GetNames(typeof(ObjectType)).Select(b => b).ToList();
            }
        }

    }
}
