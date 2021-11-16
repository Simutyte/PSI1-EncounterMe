// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private User User { get; set; }
        public ProfilePage()
        {
            InitializeComponent();

            if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserByID((int)App.s_userDb.CurrentUserId);
            }

            this.BindingContext = User;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserByID((int)App.s_userDb.CurrentUserId);
            }

            this.BindingContext = User;
        }
    }
}
