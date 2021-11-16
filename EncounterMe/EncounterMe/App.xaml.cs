// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using EncounterMe.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe
{
    public partial class App : Application
    {
        
        public static MapPinService s_mapPinService;
        public App()
        {
            InitializeComponent();
            s_mapPinService = new MapPinService();
            s_mapPinService.LoadList(); //užloadinam duomenis į PinsList.ListOfPins
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
           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
