// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllipticCurve.Utils;
using EncounterMe.Services;
using EncounterMe.Users;
using EncounterMe.Views.Popups;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
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

            OnAppearing();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.s_mapPinService.CurrentUser != null)
            {
                User = App.s_mapPinService.CurrentUser;
            }

            if (!string.IsNullOrWhiteSpace(User.PhotoPath))
            {
                ProfileImage.Source = ImageSource.FromFile(User.PhotoPath);
            }
            else
            {
                ProfileImage.Source = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";
            }
          
            this.BindingContext = User;
        }

        private async void Edit_Bio_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new EditBioPopup());
            
        }
        private async void Photo_Clicked(object sender, EventArgs e)
        {
            
            if(MediaPicker.IsCaptureSupported)
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Pick your profile photo"

                });

                
                if(result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var path = result.FullPath;
                    ProfileImage.Source = ImageSource.FromFile(path);
                    User.PhotoPath = path;
                    await App.s_mapPinService.UpdatingUser(User);
                }
                else
                {
                    await DisplayAlert("Oops", "You didn't pick a photo", "ok");
                }
            }
            else
            {
                await DisplayAlert("Oops", "We don't have permision to your photos", "ok");
            }

        }
    }
}
