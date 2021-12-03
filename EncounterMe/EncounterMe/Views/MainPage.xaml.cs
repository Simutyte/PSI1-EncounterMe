// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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

        public MainPage()
        {
            InitializeComponent();

            if (App.s_mapPinService.CurrentUser != null)
            {

                User = App.s_mapPinService.CurrentUser;
            }

            this.BindingContext = User;


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.s_mapPinService.CurrentUser != null)
            {

                User = App.s_mapPinService.CurrentUser;
            }
            this.BindingContext = User;
        }

        async void Log_Out_Button_Clicked(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync($"//{nameof(LogInPage)}");
        }
    }
}
