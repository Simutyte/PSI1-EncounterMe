// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using EncounterMe.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using EncounterMe.Services;

//Entity framework?

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public event EventHandler<RegistationEventArgs> SuccessfulRegistration;
        public RegistrationPage()
        {
            InitializeComponent();
        }

        protected virtual void OnSuccessfulRegistration(RegistationEventArgs args)
        {
            SuccessfulRegistration?.Invoke(this, args);
        }

        async void Register_Button_Clicked(object sender, EventArgs args)
        {
            var email = entryRegEmail.Text;
            var emailPatter = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"; ;

            //checking if there are empty fields
            if (string.IsNullOrWhiteSpace(entryRegUsername.Text) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(entryRegPassword.Text) || string.IsNullOrWhiteSpace(entryRegPasswordConfirm.Text))
            {
                await DisplayAlert("Entered data", "All fields must be filled", "OK");
            }
            else
            {
                //validating email
                if (!Regex.IsMatch(email, emailPatter))
                {
                    await DisplayAlert("Email", "Email is not valid", "OK");
                }
                //passed email - confirming password regex
                else
                {
                    bool passCheckResults = await checkPasswords(entryRegPassword.Text, entryRegPasswordConfirm.Text);

                    if (!passCheckResults)
                    {
                        //Clear password fields if they dont pass regex validation
                        entryRegPassword.Text = string.Empty;
                        entryRegPasswordConfirm.Text = string.Empty;
                    }
                    else
                    {
                        //if passed
                        UserDB userDB = new UserDB();
                        User user = new User
                        {
                            Username = entryRegUsername.Text,
                            Email = entryRegEmail.Text,
                            Password = entryRegPassword.Text
                        };
                        try
                        {
                            string returnValue = userDB.AddUser(user);

                            //Nedaryt su string
                            if (string.Equals(returnValue, "Sucessfully Added"))
                            {
                                await DisplayAlert("Registration", returnValue, "OK");

                                var mail = new MailService();
                                SuccessfulRegistration += mail.OnSuccessfulRegistration;

                                var eventArgs = new RegistationEventArgs();
                                eventArgs.Email = entryRegEmail.Text;
                                OnSuccessfulRegistration(eventArgs);

                                await Shell.Current.GoToAsync($"//{nameof(LogInPage)}");
                            }
                            else
                            {
                                await DisplayAlert("Registration", returnValue, "OK");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
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

            Func<string, string, bool> obj = new Func<string, string, bool>(CheckPasswordsMatch);
            bool passwordsMatch = obj.Invoke(entryRegPassword.Text, entryRegPasswordConfirm.Text);

            //bool passwordsMatch = string.Equals(entryRegPassword.Text, entryRegPasswordConfirm.Text);

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

        public static bool CheckPasswordsMatch(string pass1, string pass2)
        {
            return string.Equals(pass1, pass2) ? true : false;
        }
    }
}
