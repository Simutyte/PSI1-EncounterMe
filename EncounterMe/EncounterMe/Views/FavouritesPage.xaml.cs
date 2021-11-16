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
    public partial class FavouritesPage : ContentPage
    {
        private User User { get; }
        public FavouritesPage()
        {
            InitializeComponent();

            if (App.s_userDb.CurrentUserId != null)
            {

                User = App.s_userDb.GetUserByID((int)App.s_userDb.CurrentUserId);
            }

            if (User != null)
            {
                if (User.HasFavourite)
                {
                    //čia sąrašo priskyrimas į listview 
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
                        Text = "I'm sorry, but you don't have any favourites",
                        FontSize = 20
                    };
                    layout.Children.Add(label);
                    this.Content = layout;
                }
            }
        }
    }
}
