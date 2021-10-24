// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Pins;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddByCoordinatesPopup : PopupPage
    {
        Location _location;
        CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();

        public AddByCoordinatesPopup(Location location)
        {
            InitializeComponent();
            _location = location;
            PinLat.Text = _location.Latitude.ToString();
            PinLong.Text = _location.Longitude.ToString();
            GetAddressValue();
        }

        async void GetAddressValue()
        {
            await _checkAddressCommands.GetAddressFromCoordinates(_location);
            entryObjectCountry.Text = _checkAddressCommands.address.country;
            entryObjectCity.Text = _checkAddressCommands.address.city;
            entryObjectPostalCode.Text = _checkAddressCommands.address.postalCode;
            entryObjectStreetAndNumber.Text = _checkAddressCommands.address.street;
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(entryObjectName.Text) && string.IsNullOrWhiteSpace(entryObjectCity.Text) &&
                string.IsNullOrWhiteSpace(entryObjectCountry.Text) &&
                string.IsNullOrWhiteSpace(entryObjectStreetAndNumber.Text) && string.IsNullOrWhiteSpace(entryObjectPostalCode.Text))
            {
                await DisplayAlert("Entered data", "Name and address fields must be filled", "OK");
            }
            else
            {
                PinsList pinsList = PinsList.GetPinsList();
                PinsList list = pinsList;

                string _name = entryObjectName.Text;
                string _description = entryObjectDescription.Text;

                Address _address = new Address(entryObjectCountry.Text, entryObjectCity.Text, entryObjectPostalCode.Text, entryObjectStreetAndNumber.Text);

                TimeSpan _open = new TimeSpan(12, 00, 00);  //TODO
                TimeSpan _close = new TimeSpan(12, 00, 00);  //TODO
                WorkingHours _hours;
                _hours.openingHours = _open;
                _hours.closingTime = _close;

                Image photo = new Image();

                int _style = StyleTypePicker.SelectedIndex;
                int _type = ObjectTypePicker.SelectedIndex;

                list.AddPinByCoordinatesToList(_name, _address, _location, _type, _style, _description, _hours, photo);
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
