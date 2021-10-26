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
        //CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();
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
            if (string.IsNullOrWhiteSpace(entryObjectName.Text) && string.IsNullOrWhiteSpace(entryObjectCity.Text) &&
                string.IsNullOrWhiteSpace(entryObjectStreetAndNumber.Text) && string.IsNullOrWhiteSpace(entryObjectPostalCode.Text))
            {
                await DisplayAlert("Entered data", "Name and address fields must be filled", "OK");
            }
            else
            {
                PinsList _pinsList = PinsList.GetPinsList();
                PinsList _list = _pinsList;

                string _name = entryObjectName.Text;
                string _description = entryObjectDescription.Text;

                Address _address = new Address(entryObjectCountry.Text, entryObjectCity.Text, entryObjectPostalCode.Text, entryObjectStreetAndNumber.Text);

                TimeSpan _open = new TimeSpan(12, 00, 00);  //TODO
                TimeSpan _close = new TimeSpan(12, 00, 00);  //TODO
                WorkingHours _hours = new WorkingHours(_open, _close);

                Image _photo = new Image();

                int _style = StyleTypePicker.SelectedIndex;
                int _type = ObjectTypePicker.SelectedIndex;

                /*if (!_checkAddressCommands.CheckForExistance(address))
               {
                   await DisplayAlert("Entered data", "This address is not existing. Please try again", "OK");
               }
               else
               {*/
                _list.AddPinByAddressToList(_name, _address, _type, _style, _description, _hours, _photo);
                await PopupNavigation.Instance.PopAsync();
                //}
            }
        }
    }
}
