// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;


//TODO: maybe check name (whitespace characters?)

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        //Checks if fields are filled out correctly, return to login page if yes
        async void Register_Button_Clicked(object sender, EventArgs args)
        {

            //check if there are empty fields
            if (string.IsNullOrWhiteSpace(entryRegUsername.Text) || string.IsNullOrWhiteSpace(entryRegEmail.Text) ||
                string.IsNullOrWhiteSpace(entryRegPassword.Text) || string.IsNullOrWhiteSpace(entryRegPasswordConfirm.Text))
            {
                await DisplayAlert("Entered data", "All fields must be filled", "OK");
            }
            else
            {
                //TODO: change email validation from regex to checking if email exists
                Regex emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                bool isEmailValid = Regex.IsMatch(entryRegEmail.Text, emailRegex.ToString(), RegexOptions.IgnoreCase);

                //validating email
                if (!isEmailValid)
                {
                    await DisplayAlert("Entered data", "Email not valid", "OK");
                }
                //passed email - confirming password regex
                else
                {

                    bool passCheckResults = await checkPasswords(entryRegPassword.Text, entryRegPasswordConfirm.Text);

                    if (passCheckResults)
                    {
                        //this is temporary - until we make accounts
                        await Shell.Current.GoToAsync($"//{nameof(LogInPage)}");
                    }
                    else
                    {
                        //Clear password fields if they dont pass regex validation
                        entryRegPassword.Text = string.Empty;
                        entryRegPasswordConfirm.Text = string.Empty;
                    }
                }
            }
        }


        async Task<bool> checkPasswords(string password, string passwordConfirm)
        {

            //declaring regex for different passes
            Regex isUpperCase = new Regex(@"[A-Z]+");
            Regex isLowerCase = new Regex(@"[a-z]+");
            Regex isDigit = new Regex(@"[0-9]+");
            Regex isSymbol = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            Regex isLenght = new Regex(@".{8,20}");

            //checking if password passes these checks
            bool checkUpper = isUpperCase.IsMatch(password);
            bool checkLower = isLowerCase.IsMatch(password);
            bool checkDigit = isDigit.IsMatch(password);
            bool checkSymbol = isSymbol.IsMatch(password);
            bool checkLenght = isLenght.IsMatch(password);

            bool passwordsMatch = string.Equals(entryRegPassword.Text, entryRegPasswordConfirm.Text);

            bool passed = false;

            if (!passwordsMatch)
            {
                await DisplayAlert("Entered data", "Passwords must match", "OK");
            }
            else if (!checkUpper)
            {
                await DisplayAlert("Entered data", "Password must contain at least one upper case letter", "OK");
            }
            else if (!checkLower)
            {
                await DisplayAlert("Entered data", "Password must contain at least one lower case letter", "OK");
            }
            else if (!checkDigit)
            {
                await DisplayAlert("Entered data", "Password must contain at least one digit", "OK");
            }
            else if (!checkSymbol)
            {
                await DisplayAlert("Entered data", "Password must contain at least one special character", "OK");
            }
            else if (!checkLenght)
            {
                await DisplayAlert("Entered data", "Password lenght must be 8 - 20 characters", "OK");
            }
            else
            {
                //passed all checks
                passed = true;
            }

            //didnt pass something
            return passed;

        }
    }
}



