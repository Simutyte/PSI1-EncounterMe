// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EncounterMe.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllObjectsPage : ContentPage
    {
        //PinsList _myPinList;

        List<MapPin> ListOfPins;
        public AllObjectsPage()
        {
            InitializeComponent();
            ListOfPins = App.s_mapPinService.ListOfPins;
            BindingContext = this;
            MessagingCenter.Subscribe<AddByAddressPopup>(this, "OnPinAdded", (sender) => { OnAppearing(); });
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListOfPins = null;
            ListOfPins = App.s_mapPinService.ListOfPins;
            RefreshData();
        }

        public async void RefreshData()
        {
            listView.ItemsSource = null;
            if (ListOfPins != null)
            {
                //ListOfPins.Sort();
                await CalculateDistances();
            }

            listView.ItemsSource = GetAllObjects();
            BindingContext = this;
        }


        //Gauna pasikeitusį listOfPins pagal įvestą tekstą
        IEnumerable<MapPin> GetAllObjects(string searchText = null)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                ListOfPins.Sort();
                return ListOfPins;
            }

            var objectsQuery = from mapPin in ListOfPins
                               where mapPin.Name.ToLower().Contains(searchText.ToLower())
                               select mapPin;

            return objectsQuery.OrderBy(pin => pin.DistanceToUser);
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
            if (pinToPass == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushAsync(new IndividualObjectPage(pinToPass));
        }

        async void Favourite_clicked(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;
            var favouritePin = (MapPin)btn.CommandParameter;
            if (favouritePin != null)
            {
                if (!App.s_mapPinService.UserFavouriteMapPins.Contains(favouritePin))
                {
                    App.s_mapPinService.AddFavourite(favouritePin);
                    await DisplayAlert("Congrats", "Object " + favouritePin.Name + " was added to your favourites", "ok");
                }
                else
                {
                    await DisplayAlert("Sorry", "You have already added this object to your favourites", "ok");
                }
            }
            else
            {
                await DisplayAlert("Sorry", "Add failed bc is null", "ok");
            }
        }

        public async Task CalculateDistances()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            Location location = await Geolocation.GetLocationAsync(request);

            foreach (var pin in ListOfPins)
            {
                pin.DistanceToUser = Location.CalculateDistance(location.Latitude, location.Longitude,
                                                                pin.Latitude, pin.Longitude, DistanceUnits.Kilometers);
            }
        }
    }
}
