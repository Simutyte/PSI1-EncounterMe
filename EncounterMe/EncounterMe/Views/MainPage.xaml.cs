// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using EncounterMe.Services;
using EncounterMe.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        private User User { get; set; }
        private List<User> AllUsers;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            list.ItemsSource = null;

            if (App.s_mapPinService.CurrentUser != null)
            {

                User = App.s_mapPinService.CurrentUser;
            }

            AllUsers = App.s_mapPinService.AllUsers;
            AllUsers.Sort((a, b) => b.Score.CompareTo(a.Score));


            if (AllUsers.Count > 5)
            {
                list.ItemsSource = AllUsers.Take(5);

                var userPlace = AllUsers.FindIndex(u => u.Id == User.Id);
                Console.WriteLine("Userio indeksas " + userPlace);

                if (userPlace == 5)
                {
                    listOfUserPlace.ItemsSource = AllUsers.Skip(5).Take(1);
                    UserPlaceSeparator.Text = "Your place is " + (userPlace + 1);
                    UserPlaceSeparator.IsVisible = true;
                    listOfUserPlace.IsVisible = true;
                    UserPlaceStackLayour.IsVisible = true;
                }
                else if (userPlace > 5)
                {
                    listOfUserPlace.ItemsSource = AllUsers.Skip(userPlace - 1).Take(3);
                    UserPlaceSeparator.Text = "Your place is " + (userPlace + 1);
                    UserPlaceSeparator.IsVisible = true;
                    listOfUserPlace.IsVisible = true;
                    UserPlaceStackLayour.IsVisible = true;
                }
                else
                {
                    list.HeightRequest = 80 * 5;
                    UserPlaceSeparator.IsVisible = false;
                    listOfUserPlace.IsVisible = false;
                    UserPlaceStackLayour.IsVisible = false;
                }
            }

            else
            {

                list.HeightRequest = 80 * AllUsers.Count;
                list.ItemsSource = AllUsers;
                UserPlaceSeparator.IsVisible = false;
                listOfUserPlace.IsVisible = false;
                UserPlaceStackLayour.IsVisible = false;
            }


            this.BindingContext = User;
        }

        async void Log_Out_Button_Clicked(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync($"//{nameof(LogInPage)}");
        }

        void listView_Item_Clicked(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
