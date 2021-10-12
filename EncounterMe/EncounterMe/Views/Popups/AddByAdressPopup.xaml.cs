// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Pins;
using EncounterMe.ViewModels;
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

        [Obsolete]
        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }

        [Obsolete]
        async void Add_Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryObjectName.Text) && string.IsNullOrWhiteSpace(entryObjectCity.Text) &&
                string.IsNullOrWhiteSpace(entryObjectStreet.Text) && string.IsNullOrWhiteSpace(entryObjectNumber.Text))
            {
                await App.Current.MainPage.DisplayAlert("Entered data", "Name and address fields must be filled", "OK");
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
                var image = new Image { Source = "home.png" }; //Kazkodėl neparodo
                Image photo = image;
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
                await PopupNavigation.PopAsync();
                //}
            }
        }
    }
}
