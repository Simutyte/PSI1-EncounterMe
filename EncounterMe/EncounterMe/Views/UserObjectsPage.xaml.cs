// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserObjectsPage : ContentPage
    {
        public List<MapPin> MyMapPins { get; set; }
        public UserObjectsPage()
        {
            InitializeComponent();
           
            MyMapPins = App.s_mapPinService.UserOwnerMapPins;
            listView.ItemsSource = App.s_mapPinService.UserOwnerMapPins; 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MyMapPins = App.s_mapPinService.UserOwnerMapPins;
        }

        async void Delete_clicked(object sender, EventArgs e)
        {
            Console.WriteLine("pries viska");
            var btn = (ImageButton)sender;
            Console.WriteLine("po imgbutton");
            var deletePin = (MapPin)btn.CommandParameter;
            Console.WriteLine("po priskyrimo");
            if (deletePin != null)
            {
                await App.s_mapPinService.DeleteOwnedObjects(deletePin);
                //App.s_userDb.DeleteFavPin(deletePin);
               
                await DisplayAlert("Deleted", "Your object was deleted", "ok");
                OnAppearing();
                //AfterDeleted();
            }
            else
            {
                await DisplayAlert("Sorry", "Delete failed bc is null", "ok");
            }

        }
    }
}
