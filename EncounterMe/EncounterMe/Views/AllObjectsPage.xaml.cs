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
        public ObservableRangeCollection<MapPin> allObjectsCollection { get; set; }

        PinsList myPinList;

        public AllObjectsPage()
        {
            InitializeComponent();
            PinsList pinsList = PinsList.GetPinsList();
            myPinList = pinsList;
            Adding(myPinList.list);
            myPinList.list.Sort();
            listView.ItemsSource = GetAllObjects();
            this.BindingContext = this;


        }

        //Pridedu bendram patikrinimui
        public void Adding(List<MapPin> list)
        {
            Address address = new Address();
            address.city = "Kaunas";
            address.country = "Lietuva";
            address.street = "Algimanto g. 13";

            list.Add(new MapPin("Muziejus test",address));
            list.Add(new MapPin("Paminklas testPam", address));
            list.Add(new MapPin("Parkas testParkas", address));
            list.Add(new MapPin("Dvaras Kablys", address));
            list.Add(new MapPin("Akvariumas", address));
        }

        //Gauna pasikeitusį list pagal įvestą tekstą
        IEnumerable<MapPin> GetAllObjects(string searchText = null)
        {
            if(string.IsNullOrEmpty(searchText))
            {
                return myPinList.list;
            }

            var objectsQuery = from mapPin in myPinList.list
                               where mapPin.name.ToLower().StartsWith(searchText.ToLower())
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
