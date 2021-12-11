// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using EncounterMe.Pins;
using EncounterMe.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddByAdressPopup : PopupPage
    {
        static CheckAddressCommands _checkAddressCommands = new CheckAddressCommands();
        public AddByAdressPopup()
        {
            InitializeComponent();
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        Func<string, string, string, string, Task<bool>> AddressExist = async delegate (string country, string city, string code, string street)
        {
            Address _address = new Address(country, city, code, street);
            await _checkAddressCommands.GetCoordinatesFromAddress(_address);
            return _checkAddressCommands.ExistAddress;
        };

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
                if (await AddressExist(entryObjectCountry.Text, entryObjectCity.Text, entryObjectPostalCode.Text, entryObjectStreetAndNumber.Text))
                {
                    Address _address = new Address(entryObjectCountry.Text, entryObjectCity.Text, entryObjectPostalCode.Text, entryObjectStreetAndNumber.Text);
                    await _checkAddressCommands.GetCoordinatesFromAddress(_address);

                    string _open = entryOpenTime.Time.Hours + ":" + entryOpenTime.Time.Minutes;
                    string _close = entryCloseTime.Time.Hours + ":" + entryCloseTime.Time.Minutes;

                    int _style = StyleTypePicker.SelectedIndex;
                    int _type = ObjectTypePicker.SelectedIndex;

                    MapPin MapPin = new MapPin(entryObjectName.Text, entryObjectDescription.Text, _address, _type, _style,
                                               _open, _close, _checkAddressCommands.Location.Latitude, _checkAddressCommands.Location.Longitude, entryObjectImage.Text,
                                                App.s_mapPinService.CurrentUser.Id);

                    var notificationsService = new NotificationsService();
                    App.s_mapPinService.PinAdded += notificationsService.OnPinAdded;

                     App.s_mapPinService.TryToAdd(MapPin);
                    //App.s_mapPinService.LoadOwnerObjects();
                    await PopupNavigation.Instance.PopAllAsync();
                }
                else
                {
                    await DisplayAlert("Entered data", "Address is not existing", "OK");
                }

            }
        }
    }
}
