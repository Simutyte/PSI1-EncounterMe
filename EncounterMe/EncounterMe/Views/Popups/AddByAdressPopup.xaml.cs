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
                PinsList pinsList = PinsList.GetPinsList();
                PinsList list = pinsList;

                string name = entryObjectName.Text;
                Address address = new Address();
                //address.country = entryObjectCountry.Text;
                address.city = entryObjectCity.Text;
                //address.postalCode = entryObjectPostalCode.Text;
                //address.street = entryObjectStreetAndNumber.Text;

                TimeSpan open = new TimeSpan(12, 00, 00);  //TODO
                TimeSpan close = new TimeSpan(12, 00, 00);  //TODO
                string description = entryObjectDescription.Text;
                Image photo = new Image();
                int style = StyleTypePicker.SelectedIndex;
                int type = ObjectTypePicker.SelectedIndex;
                WorkingHours hours;
                hours.openingHours = open;
                hours.closingTime = close;

                /*if (!_checkAddressCommands.CheckForExistance(address))
               {
                   await DisplayAlert("Entered data", "This address is not existing. Please try again", "OK");
               }
               else
               {*/
                list.AddPinByAddressToList(name, address, type, style, description, hours, photo);
                await PopupNavigation.Instance.PopAsync();
                //}
            }
        }
    }
}
