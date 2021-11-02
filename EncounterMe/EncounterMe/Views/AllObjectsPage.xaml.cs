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
            Xamarin.Essentials.Location location = new Xamarin.Essentials.Location();
            location.Latitude = 10;
            location.Longitude = 10;


            list.Add(new MapPin("Muziejus", new Address { Country = "Lithuania", City = "Kaunas", Street = "Kauno g. 15A" }, location, new WorkingHours { }, ObjectType.Museum));
            list.Add(new MapPin("Basanaviciaus paminklas", new Address { Country = "Lithuania", City = "Marijampole", Street = "Basanaviciaus g. 4" }, location, new WorkingHours { }, ObjectType.Monument));
            list.Add(new MapPin("Ramybes Parkas", new Address { Country = "Lithuania", City = "Kaunas", Street = "Kauno g. 7" }, location, new WorkingHours { }, ObjectType.Park));
            list.Add(new MapPin("Petro Povilo baznycia", new Address { Country = "Lithuania", City = "Vilnius", Street = "Vilniaus g. 4" }, location, new WorkingHours { }, ObjectType.Church));
            list.Add(new MapPin("Didysis akvariumas", new Address { Country = "Lithuania", City = "Plunge", Street = "Parko g. 14" }, location, new WorkingHours { }, ObjectType.Entertainment));
        }

        //Gauna pasikeitusį listOfPins pagal įvestą tekstą
        IEnumerable<MapPin> GetAllObjects(string searchText = null)
        {
            if(string.IsNullOrEmpty(searchText))
            {
                return _myPinList.ListOfPins;
            }

            var objectsQuery = from mapPin in _myPinList.ListOfPins
                               where mapPin.Name.ToLower().Contains(searchText.ToLower())
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

        async void listView_ItemSelected(object sender, ItemTappedEventArgs e)
        {

            var pinToPass = ((ListView)sender).SelectedItem as MapPin;
            if (pinToPass == null)
            {
                return;
            }

            //await Shell.Current.DisplayAlert("Map Pin selected", pinToPass.Name, "okey");
            await Shell.Current.Navigation.PushAsync(new IndividualObjectPage(pinToPass));


        }

        void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

       async void More_Info_Clicked(object sender, EventArgs e)
        {
            var pinToPass = ((MenuItem)sender).BindingContext as MapPin;
            if(pinToPass == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushAsync(new IndividualObjectPage(pinToPass));
        }

    }
}
