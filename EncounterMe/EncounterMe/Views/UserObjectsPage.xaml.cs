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

            OnAppearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.s_mapPinService.UserOwnerMapPins.Count > 0)
            {
                this.Content = MainStackLayout;
                listView.ItemsSource = App.s_mapPinService.UserOwnerMapPins;
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
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "I'm sorry, but you haven't created your \nobjects yet",
                    FontSize = 20
                };
                layout.Children.Add(label);

                this.Content = layout;
            }
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
