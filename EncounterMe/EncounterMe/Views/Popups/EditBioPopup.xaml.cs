// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;
using EncounterMe.Users;

namespace EncounterMe.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBioPopup : PopupPage
    {
        private User User { get; set; }
        public EditBioPopup()
        {
            InitializeComponent();

            /*if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserByID((int)App.s_userDb.CurrentUserId);
            }

            if(!string.IsNullOrWhiteSpace(User.AboutMe))
            {
                editorAboutMe.Text = User.AboutMe;
            }*/
           

            
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(editorAboutMe.Text))
            {
                await DisplayAlert("Info", "Please add information about you", "ok");
            }
            else
            {
                User.AboutMe = editorAboutMe.Text;
                Console.WriteLine(User.AboutMe);
               // App.s_userDb.UpdateUser(User);
                await PopupNavigation.Instance.PopAsync();
               
            }
        }
    }
}
