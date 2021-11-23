// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddObjectPopup : PopupPage
    {
        public AddObjectPopup()
        {
            InitializeComponent();
        }

        async void Add_By_Adress_Button_Clicked(object sender, EventArgs args)
        {
            await PopupNavigation.Instance.PushAsync(new AddByAdressPopup());
        }

        async void Add_By_Pin_Button_Clicked(object sender, EventArgs args)
        {
            await AppShell.Current.GoToAsync($"//home/tab/MapPage?pin=true");
            await PopupNavigation.Instance.PopAsync();
        }
        async void Add_By_Current_Coordinates_Button_Clicked(object sender, EventArgs args)
        {
            var requestLocation = new GeolocationRequest(GeolocationAccuracy.Default);
            var location = await Geolocation.GetLocationAsync(requestLocation);
            await PopupNavigation.Instance.PushAsync(new AddByCoordinatesPopup(location));
        }
        
    }
}
