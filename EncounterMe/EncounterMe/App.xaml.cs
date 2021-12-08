// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using EncounterMe.Helpers;
using EncounterMe.Services;
using EncounterMe.Users;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe
{
    public partial class App : Application
    {
        
        public static MapPinService s_mapPinService;
        public static UserDB s_userDb;
        public App()
        {
            InitializeComponent();
            s_mapPinService = new MapPinService();
            s_mapPinService.LoadList(); //užloadinam duomenis į PinsList.ListOfPins
            s_mapPinService.LoadUsers();
            TheTheme.SetTheme();
            SetCultureToUSEnglish();
            MainPage = new AppShell();
        }
        
        private void SetCultureToUSEnglish()
        {
            CultureInfo englishUSCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishUSCulture;
            CultureInfo.DefaultThreadCurrentUICulture = englishUSCulture;
        }


        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
    }
}
