// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Pins;
using EncounterMe.Services;
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
            entryObjectCountry.Text = _checkAddressCommands.Address.Country;
            entryObjectCity.Text = _checkAddressCommands.Address.City;
            entryObjectPostalCode.Text = _checkAddressCommands.Address.PostalCode;
            entryObjectStreetAndNumber.Text = _checkAddressCommands.Address.Street;
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(entryObjectName.Text) || string.IsNullOrWhiteSpace(entryObjectCity.Text) ||
                string.IsNullOrWhiteSpace(entryObjectCountry.Text) || string.IsNullOrWhiteSpace(entryObjectDescription.Text) ||
                string.IsNullOrWhiteSpace(entryObjectStreetAndNumber.Text) || string.IsNullOrWhiteSpace(entryObjectPostalCode.Text))
            {
                await DisplayAlert("Entered data", "Fields were not generated automatically, please, fill them manually", "OK");
            }
            else
            {

                Address _address = new Address(entryObjectCountry.Text, entryObjectCity.Text, entryObjectPostalCode.Text, entryObjectStreetAndNumber.Text);

                string _open = entryOpenTime.Time.Hours + ":" + entryOpenTime.Time.Minutes;
                string _close = entryCloseTime.Time.Hours + ":" + entryCloseTime.Time.Minutes;

                int _style = StyleTypePicker.SelectedIndex;
                int _type = ObjectTypePicker.SelectedIndex;

                MapPin MapPin = new MapPin(entryObjectName.Text, entryObjectDescription.Text, _address, _type, _style,
                                           _open, _close, _location.Latitude, _location.Longitude, entryObjectImage.Text);

                var notificationsService = new NotificationsService();
                App.s_mapPinService.PinAdded += notificationsService.OnPinAdded;

                App.s_mapPinService.TryToAdd(MapPin);

                await PopupNavigation.Instance.PopAllAsync();
            }
        }
    }
}
