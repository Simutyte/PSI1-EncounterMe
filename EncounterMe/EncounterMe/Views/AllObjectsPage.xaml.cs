// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using EncounterMe.Pins;
using EncounterMe.Views.Popups;
using MvvmHelpers;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllObjectsPage : ContentPage
    {
        PinsList _myPinList;

        public AllObjectsPage()
        {
            InitializeComponent();
            PinsList pinsList = PinsList.GetPinsList();
            _myPinList = pinsList;
            Adding(_myPinList.ListOfPins);
            _myPinList.ListOfPins.Sort();
            listView.ItemsSource = GetAllObjects();
            BindingContext = this;
        }

        //Pridedu bendram patikrinimui
        public void Adding(List<MapPin> list)
        {
            Address address = new Address
            {
                City = "Kaunas",
                Country = "Lietuva",
                Street = "Algimanto g. 13"
            };

            list.Add(new MapPin("Muziejus test",address));
            list.Add(new MapPin("Paminklas testPam", address));
            list.Add(new MapPin("Parkas testParkas", address));
            list.Add(new MapPin("Dvaras Kablys", address));
            list.Add(new MapPin("Akvariumas", address));
        }

        //Gauna pasikeitusį listOfPins pagal įvestą tekstą
        IEnumerable<MapPin> GetAllObjects(string searchText = null)
        {
            if(string.IsNullOrEmpty(searchText))
            {
                return _myPinList.ListOfPins;
            }

            var objectsQuery = from mapPin in _myPinList.ListOfPins
                               where mapPin.Name.ToLower().StartsWith(searchText.ToLower())
                               select mapPin;
            return objectsQuery;           
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            listView.ItemsSource = GetAllObjects();
            listView.EndRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listView.ItemsSource = GetAllObjects(e.NewTextValue);
        }

        async void Add_Object_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddObjectPopup());
        }
    }
}
