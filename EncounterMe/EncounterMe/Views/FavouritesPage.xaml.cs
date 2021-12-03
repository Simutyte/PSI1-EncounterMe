// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EncounterMe.Pins;
using EncounterMe.Services;
using EncounterMe.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritesPage : ContentPage
    {
        public List<MapPin> FavouriteMapPinList { get; set; }

        private User User { get; set; }
        public FavouritesPage()
        {
            InitializeComponent();  
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = null;

            if (App.s_mapPinService.UserFavouriteMapPins.Count > 0)
            {
                this.Content = MainStackLayout;
                listView.ItemsSource = App.s_mapPinService.UserFavouriteMapPins;
            }
            else
            {
                var layout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                Label label = new Label
                {
                    Text = "I'm sorry, but you don't have any favourites",
                    FontSize = 20
                };
                layout.Children.Add(label);

                this.Content = layout;
            }
        }


         async void Delete_clicked(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;
            var deletePin = (MapPin)btn.CommandParameter;
            if (deletePin != null)
            {
                App.s_mapPinService.DeleteFavourite(deletePin);
                await DisplayAlert("Deleted", "Your object was deleted", "ok");
                OnAppearing();
                //AfterDeleted();
            }
            else
            {
                await DisplayAlert("Sorry", "Delete failed bc is null", "ok");
            }

        }

        void listView_ItemSelected(object sender, ItemTappedEventArgs e)
        {

            ((ListView)sender).SelectedItem = null;

        }

        void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}

