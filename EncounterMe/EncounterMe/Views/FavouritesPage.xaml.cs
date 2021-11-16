// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    
                    if(App.s_mapPinService.FavouritePins.Value.Count > 0)
                    {
                        foreach(var MapPin in App.s_mapPinService.FavouritePins.Value)
                        {
                            FavouriteMapPinList.Value.Add(MapPin);
                        }
                       
                        listView.ItemsSource = FavouriteMapPinList.Value;
                        BindingContext = this;
                    }
                    else
                    {
                        DisplayAlert("nk nera", "nk nera", "ok");
                    }
                    
                        
                    
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
