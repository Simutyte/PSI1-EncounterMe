// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Windows.Input;
using EncounterMe.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell: Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;            
        }

        Dictionary<String, System.Type> routes = new Dictionary<string, System.Type>();

        public ICommand NavigateCommand => new Command(Navigate);
        
        private async void Navigate(object route)
        {
            ShellNavigationState state = Shell.Current.CurrentState;
            Console.WriteLine(state.ToString());
            Console.WriteLine(state.Location.ToString());
            Console.WriteLine(route.ToString());
            await Shell.Current.GoToAsync($"{state.Location}/{route.ToString()}");
            Shell.Current.FlyoutIsPresented = false;
           
        }

        void RegisterRoutes()
        {
            routes.Add("aboutUs", typeof(AboutUsPage));
            routes.Add("settings", typeof(SettingsPage));
            routes.Add("MainPage", typeof(MainPage));
            routes.Add("RegistrationPage", typeof(RegistrationPage));
            routes.Add("MapPage", typeof(MapPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

       


    }
}
