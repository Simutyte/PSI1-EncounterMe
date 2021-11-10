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

            if(_myPinList.ListOfPins != null)
            {
                _myPinList.ListOfPins.Sort();

            }

            listView.ItemsSource = GetAllObjects();
            BindingContext = this;

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
