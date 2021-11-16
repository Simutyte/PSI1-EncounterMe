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
        private User User { get; }
        public FavouritesPage()
        {
            InitializeComponent();
            FavouriteMapPinList = new Lazy<List<MapPin>>();
            BindingContext = this;
            if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserWithChildren((int)App.s_userDb.CurrentUserId);
            }

           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(User != null)
            {
                if (User.HasFavourite)
                {
                    UpdateFavourites();
                    listView.ItemsSource = FavouriteMapPinList.Value;
                    BindingContext = this;

                    //if (App.s_mapPinService.FavouritePins.Value.Count > 0)
                    //{
                        /*if (FavouriteMapPinList.IsValueCreated)
                            FavouriteMapPinList.Value.Clear();

                        foreach(var MapPin in App.s_mapPinService.FavouritePins.Value)
                        {
                            FavouriteMapPinList.Value.Add(MapPin);
                        }
                       
                        listView.ItemsSource = FavouriteMapPinList.Value;*/
                       /* UpdateFavourites();
                        listView.ItemsSource = FavouriteMapPinList.Value;
                        BindingContext = this;
                    }
                    else
                    {
                        DisplayAlert("nk nera", "nk nera", "ok");
                    }*/
                    
                        
                    
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
            Console.WriteLine(User.FavouriteObjects.Count);
            Thread thread1 = new Thread(() =>
            {
                try
                {
                    //Console.WriteLine("Esu thread1 try");
                    if (FavouriteMapPinList.IsValueCreated)
                        FavouriteMapPinList.Value.Clear();

                    foreach (var pin2 in User.FavouriteObjects)
                    {
                        //Console.WriteLine("Esu thread1 foreeach viduj pries await");
                        var mapPin = ApiMapPinService.GetMapPin(pin2.ObjectId).Result;

                        //Console.WriteLine("Esu thread1 foreeach viduj po await");

                        FavouriteMapPinList.Value.Add( mapPin);
                        //Console.WriteLine("Esu thread1 foreeach viduj po pridejimo");
                    }

                    success = true;
                    Console.WriteLine("Esu thread1 po success pakeitimo");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex);
                    success = false;
                }
            });

            Thread thread2 = new Thread(() =>
            {
                //Console.WriteLine("Esu thread2 ");
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
    }

}
