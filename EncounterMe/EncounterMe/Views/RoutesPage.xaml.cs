// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Pins;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutesPage : ContentPage
    {
        PinsList _myPinList;
        public RoutesPage()
        {
            InitializeComponent();

            PinsList pinsList = PinsList.GetPinsList();
            _myPinList = pinsList;

            RoutesListView.ItemsSource = GetAllObjects();
            // BindingContext = this;
        }

        private string _textValue;
        

        IEnumerable<MapPin> GetAllObjects(string searchText = null)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                _textValue = string.Empty;
                return _myPinList.ListOfPins;
            }
            _textValue = searchText;
            var objectsQuery = from mapPin in _myPinList.ListOfPins
                               where mapPin.Address.City != null && mapPin.Address.City.ToLower().Contains(searchText.ToLower())
                               select mapPin;
            return objectsQuery;
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            RoutesListView.ItemsSource = GetAllObjects();
            RoutesListView.EndRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            RoutesListView.ItemsSource = GetAllObjects(e.NewTextValue);
        }

        void Picker_index_Changed(object sender, EventArgs e)
        {
            int selectedIndex = ObjectTypePicker.SelectedIndex;
            if (selectedIndex != -1)
                RoutesListView.ItemsSource = GetSpecificObjects(selectedIndex);
        }

        IEnumerable<MapPin> GetSpecificObjects(int index)
        {
            var objectsQuery = from mapPin in _myPinList.ListOfPins
                               where index == (int)mapPin.Type
                               select mapPin;
            return objectsQuery;
        }

        private void Cross_Button_Clicked(object sender, EventArgs e)
        {
            ObjectTypePicker.SelectedIndex = -1;
            RoutesListView.ItemsSource = GetAllObjects(_textValue);
        }
    }
}
