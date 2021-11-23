// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Android.Content;
using EncounterMe.Droid.CustomRenderer;
using EncounterMe.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LogInCustomEntry), typeof(LogInEntryRenderer))]
namespace EncounterMe.Droid.CustomRenderer
{
    public class LogInEntryRenderer : EntryRenderer
    {

        public LogInEntryRenderer(Context contex) : base(contex)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                //Control.SetTextColor(Android.Graphics.Color.DarkGreen);             
            }
        }
    }
}
