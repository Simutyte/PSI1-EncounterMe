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
            //_checkAddressCommands.GetAddress(_location);
            //await _checkAddressCommands.GetAddressFromCoordinates(_location);
            Something();
            
        }

        async void Something()
        {
            await _checkAddressCommands.GetAddressFromCoordinates(_location);
            entryObjectCity.Text = _checkAddressCommands.City;
            entryObjectStreet.Text = _checkAddressCommands.Street;
            Console.WriteLine(_checkAddressCommands.City + " -- " + _checkAddressCommands.Street);
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

         async void Add_Button_Clicked(object sender, EventArgs e)
         {

            if (string.IsNullOrWhiteSpace(entryObjectName.Text) && string.IsNullOrWhiteSpace(entryObjectCity.Text) &&
                string.IsNullOrWhiteSpace(entryObjectStreet.Text) && string.IsNullOrWhiteSpace(entryObjectNumber.Text))
            {
                await DisplayAlert("Entered data", "Name and address fields must be filled", "OK");
            }
            else
            {
                PinsList pinsList = PinsList.GetPinsList();
                PinsList list = pinsList;

                string name = entryObjectName.Text;
                string address = entryObjectCity.Text + " " + entryObjectStreet.Text + " " + entryObjectNumber.Text;
                TimeSpan open = new TimeSpan(12, 00, 00);  //TODO
                TimeSpan close = new TimeSpan(12, 00, 00);  //TODO
                string description = entryObjectDescription.Text;
                Image photo = new Image();
                int style = StyleTypePicker.SelectedIndex;
                int type = ObjectTypePicker.SelectedIndex;
                WorkingHours hours;
                hours.openingHours = open;
                hours.closingTime = close;

                list.AddPinByAddressToList(name, address, type, style, description, hours, photo);
                await PopupNavigation.Instance.PopAsync();
            }
         }

    }
}
