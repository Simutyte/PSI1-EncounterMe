// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutesPage : ContentPage
    {
        private PinsList _myPinList;
        private string _textValue;
        public RoutesPage()
        {
            InitializeComponent();

            //PinsList pinsList = PinsList.GetPinsList();
            //_myPinList = pinsList;
            ListOfPins = App.s_mapPinService.ListOfPins;
            RoutesListView.ItemsSource = GetAllObjects();
        }
        
        IEnumerable<Route> GetAllObjects(string searchText = null)
        {
            var objectsByCityAndStyle = ListOfPins.GroupBy(e => new { e.Address.City, e.StyleType }).Where(e => e.Count() > 1).Select(e => new Route
            {
                City = e.Key.City,
                Style = e.Key.StyleType,
                MapPins = e.AsEnumerable(),
                Count = e.Count()
            });

            if (string.IsNullOrEmpty(searchText))
            {
                _textValue = string.Empty;
                return objectsByCityAndStyle;
            }
            else
            {
                _textValue = searchText;
                return objectsByCityAndStyle.Where(e => e.City.ToLower().Contains(searchText.ToLower()));
            }
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            RoutesListView.ItemsSource = GetAllObjects();
            RoutesListView.EndRefresh();
        }

        private async void listView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            var objectGroup = ((ListView)sender).SelectedItem as Route;
            if (objectGroup == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushAsync(new LoadRoutePage(objectGroup.MapPins));
        }

        void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    RoutesListView.ItemsSource = GetAllObjects(e.NewTextValue);
        //}

        //void Picker_index_Changed(object sender, EventArgs e)
        //{
        //    int selectedIndex = ObjectTypePicker.SelectedIndex;
        //    if (selectedIndex != -1)
        //        RoutesListView.ItemsSource = GetSpecificObjects(selectedIndex);
        //}

        //IEnumerable<Route> GetSpecificObjects(int index)
        //{
        //    var objectsQuery = GetAllObjects().Where(e => (int)e.Style == index);
        //    return objectsQuery;
        //}

        //private void Cross_Button_Clicked(object sender, EventArgs e)
        //{
        //    ObjectTypePicker.SelectedIndex = -1;
        //    RoutesListView.ItemsSource = GetAllObjects(_textValue);
        //}
    }
}
