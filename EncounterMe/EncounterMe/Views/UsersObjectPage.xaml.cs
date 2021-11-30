// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersObjectPage : ContentPage
    {
        public List<MapPin> MyMapPins { get; set; }
        public UsersObjectPage()
        {
            InitializeComponent();
           
            MyMapPins = App.s_mapPinService.UserOwnerMapPins;
            listView.ItemsSource = MyMapPins;
        }
    }
}
