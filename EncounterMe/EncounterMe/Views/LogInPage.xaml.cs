// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using EncounterMe.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        async void Log_In_Button_Clicked(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

            //PinsList classToGetList = PinsList.GetPinsList();

            PinsList classToGetList = PinsList.Instance;
            classToGetList.GetListOfPinsFromFile();

            if (string.IsNullOrWhiteSpace(entryLogUsername.Text) || string.IsNullOrWhiteSpace(entryLogPassword.Text))
            {
                await DisplayAlert("Entered data", "All fields must be filled", "OK");
            }
            else
            {

                UserDB userDB = new UserDB();

                if (userDB.LoginValidate(entryLogUsername.Text, entryLogPassword.Text))
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    await DisplayAlert("Log In", "Such user does not exist, please check your information or register", "OK");
                    entryLogPassword.Text = string.Empty;
                    entryLogUsername.Text = string.Empty;
                }
            }
        }

        private async void Tapped_Registration(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
            entryLogPassword.Text = string.Empty;
            entryLogUsername.Text = string.Empty;
        }
    }
}
