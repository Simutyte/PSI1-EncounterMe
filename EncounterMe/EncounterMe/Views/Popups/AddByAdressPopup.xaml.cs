// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using EncounterMe.Pins;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddByAdressPopup : PopupPage
    {
        CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();
        public AddByAdressPopup()
        {
            InitializeComponent();
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryObjectName.Text) || string.IsNullOrWhiteSpace(entryObjectCountry.Text) ||
                string.IsNullOrWhiteSpace(entryObjectCity.Text) || string.IsNullOrWhiteSpace(entryObjectDescription.Text) ||
                string.IsNullOrWhiteSpace(entryObjectStreetAndNumber.Text) || string.IsNullOrEmpty(entryObjectPostalCode.Text))
            {
                await DisplayAlert("Entered data", "Name, address and description fields must be filled", "OK");
            }
            else
            {
                Address _address = new Address(entryObjectCountry.Text, entryObjectCity.Text, entryObjectPostalCode.Text, entryObjectStreetAndNumber.Text);
                await _checkAddressCommands.GetCoordinatesFromAddress(_address);

                string _open = entryOpenTime.Time.Hours + ":" + entryOpenTime.Time.Minutes;
                string _close = entryCloseTime.Time.Hours + ":" + entryCloseTime.Time.Minutes;

                int _style = StyleTypePicker.SelectedIndex;
                int _type = ObjectTypePicker.SelectedIndex;

                MapPin MapPin = new MapPin(entryObjectName.Text, entryObjectDescription.Text, _address, _type, _style,
                                           _open, _close, _checkAddressCommands.Location.Latitude, _checkAddressCommands.Location.Longitude, entryObjectImage.Text);

                App.s_mapPinService.TryToAdd(MapPin);

                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
