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
        public Lazy<List<MapPin>> FavouriteMapPinList { get; set; }
        private bool success = false;
        private User User { get; set;  }
        public FavouritesPage()
        {
            InitializeComponent();
            FavouriteMapPinList = new Lazy<List<MapPin>>();
            BindingContext = this;
            if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserByID((int)App.s_userDb.CurrentUserId);
            }

           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserByID((int)App.s_userDb.CurrentUserId);
            }

            if (User != null)
            {
                if (User.HasFavourite)
                {
                    UpdateFavourites();
                    listView.ItemsSource = FavouriteMapPinList.Value;
                    BindingContext = this;
                    
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
            

        }

        private void UpdateFavourites()
        {
            
            Thread thread1 = new Thread(() =>
            {
                try
                {
                    
                    if (FavouriteMapPinList.IsValueCreated)
                        FavouriteMapPinList.Value.Clear();

                    List<FavouritePin> AllPins = App.s_userDb.GetAllFavPins();
                    foreach(FavouritePin pin in AllPins)
                    {
                        if(pin.UserId == (int)App.s_userDb.CurrentUserId)
                        {
                            var mapPin = ApiMapPinService.GetMapPin(pin.ObjectId).Result;
                            FavouriteMapPinList.Value.Add(mapPin);
                        }
                    }

                    success = true;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    success = false;
                }
            });

            Thread thread2 = new Thread(() =>
            {
                
                if (success)
                {
                    DisplayAlert("Favourite objects", "Your favourite objects were updated", "ok");
                }
                else
                {
                    DisplayAlert("Favourite objects", "Sorry, something went wrong, we can't show your favourite objects", "ok");
                }
            });

            
            thread1.Start();
            thread1.Join();
            thread2.Start();

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
