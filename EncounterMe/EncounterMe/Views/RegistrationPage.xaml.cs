// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        async void Register_Button_Clicked(object sender, EventArgs args)
        {
            if (string.IsNullOrWhiteSpace(entryRegUsername.Text) || string.IsNullOrWhiteSpace(entryRegEmail.Text) ||
                string.IsNullOrWhiteSpace(entryRegPassword.Text) || string.IsNullOrWhiteSpace(entryRegConfirmPassword.Text))
            {
                await DisplayAlert("Entered data", "All fields must be filled", "OK");
            }
            else if (!string.Equals(entryRegPassword.Text, entryRegConfirmPassword.Text))
            {
                await DisplayAlert("Password", "Passwords must match", "OK");

                entryRegPassword.Text = string.Empty;
                entryRegConfirmPassword.Text = string.Empty;

            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LogInPage)}");
            }
        }
    }
}
