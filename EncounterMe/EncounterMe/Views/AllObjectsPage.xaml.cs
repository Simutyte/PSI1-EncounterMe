﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using EncounterMe.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllObjectsPage : ContentPage
    {
        public AllObjectsPage()
        {
            InitializeComponent();
        }

        async void Add_Object_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddObjectPopup());

        }
    }
}
