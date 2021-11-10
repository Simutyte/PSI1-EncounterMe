﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using EncounterMe.Services;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using EncounterMe.Views.Popups;
using System.Net;
using System.IO;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


//TODO: make route interactive (+rep jei be additional json parse)


namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(Title), "title")]
    public partial class MapPage : ContentPage
    {

        private Location _location = new Location();

        private PinsList _myPinList;

        private Location title = null;

        public Location Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }


        private bool _chosenLocationPermission;
        private bool _chosenGpsPermission;

        public MapPage()
        {
            if (title != null)
            {
                DisplayRoute(title);
                title = null;
            }
                

            _chosenLocationPermission = false;
            _chosenGpsPermission = false;

            InitializeComponent();
            DisplayCurrentLocation();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DisplayCurrentLocation();
            AnimationView.IsVisible = false;
            CenterPin.IsVisible = false;
            GenerateMapPins();
        }


        public void GenerateMapPins()
        {
            PinsList pinsList = PinsList.GetPinsList();
            _myPinList = pinsList;

            foreach (MapPin mapPin in _myPinList.ListOfPins)
            {
                if (mapPin.Pin != null)
                {
                    MyMap.Pins.Add(mapPin.Pin);
                }
            }
        }

        private async void DisplayCurrentLocation()
        {
            try
            {
                //Trying to display current location. GeolocationAccuracy.Default for quicker initialization
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    //Moving to location and starting live tracking
                    UpdateCurrentLocation(location);
                    TrackingLiveLocation();
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Alert", "This feature is not supported on your device", "Ok");
            }
            catch (FeatureNotEnabledException)
            {
                //If user was not yet prompted to enable gps
                if (_chosenGpsPermission == false)
                {
                    PromptToEnableGPS();
                }
            }
            catch (PermissionException)
            {
                //If location permission is not already enabled and not yet chosen, prompt the user and call the method once again
                if (_chosenLocationPermission == false)
                {
                    PromptToEnableLocationPermission();
                }
            }
            catch (Exception)
            {

            }
        }
        async void PromptToEnableLocationPermission()
        {
            _chosenLocationPermission = true;
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            DisplayCurrentLocation();
        }

        async void PromptToEnableGPS()
        {
            //If there was no button was pressed (tapped outside the alert box), answer = false, user will not get live tracking
            bool answer = await DisplayAlert("Location request", "Turn on your phone's location service for better performance.", "OK", "Maybe later");
            _chosenGpsPermission = true;

            if (answer)
            {
                //Opens settings and starts tracking live location
                IGpsDependencyService GpsDependency = DependencyService.Get<IGpsDependencyService>();
                GpsDependency.OpenSettings();
                DisplayCurrentLocation();
            }
        }

        //Constantly runs in the background, tracks current location and updates it
        private void TrackingLiveLocation()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Task.Run(async () =>
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Default);
                    var location = await Geolocation.GetLocationAsync(request);
                    UpdateCurrentLocation(location);
                });
                return true;
            });
        }

        void UpdateCurrentLocation(Location loc)
        {
            Position p = new Position(loc.Latitude, loc.Longitude);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(1));
            MyMap.MoveToRegion(mapSpan);
        }

        void Add_Pin_Button_Clicked(object sender, EventArgs e)
        {
            AnimationView.IsVisible = true;
            CenterPin.IsVisible = true;
        }

        //Kad dar kartą paspaudus addPin mygtuką vėl atsirastų animacija
        void animationView_OnFinishedAnimation(object sender, EventArgs e)
        {
            AnimationView.PlayAnimation();
            AnimationView.PauseAnimation();
            AnimationView.IsVisible = false;
        }

        async void Confirm_Add_Pin_Button_Clicked(object sender, EventArgs e)
        {
            CenterPin.IsVisible = false;
            AnimationView.PlayAnimation();

            if (_location != null)
            {
                await PopupNavigation.Instance.PushAsync(new AddByCoordinatesPopup(_location));
            }
            else
            {
                await DisplayAlert("Location", "Something went wrong with coordinates", "okey");
            }

        }

        //Updates current center position when map is moved and saves lat. and long. to location
        void Position_Map_Property_Changed(object sender, EventArgs e)
        {
            var m = (Xamarin.Forms.Maps.Map)sender;
            if (m.VisibleRegion != null)
            {
                _location.Latitude = m.VisibleRegion.Center.Latitude;
                _location.Longitude = m.VisibleRegion.Center.Longitude;
            }
        }


        //Keep in mind, that this API uses format long:lat
        public async void DisplayRoute(Location endLocation)
        {
            //Requesting current location
            var requestLocation = new GeolocationRequest(GeolocationAccuracy.Default);
            var startLocation = await Geolocation.GetLocationAsync(requestLocation);

            string result = "";

            if (endLocation != null)
            {
                try
                {
                    //URL to API
                    string URL = $"http://api.openrouteservice.org/v2/directions/driving-car?" +
                    $"api_key=5b3ce3597851110001cf62480ee65daaadbe486f9218ad7d5288ad0a" +
                    $"&start={startLocation.Longitude},{startLocation.Latitude}" +
                    $"&end={endLocation.Longitude},{endLocation.Latitude}";

                    //Getting a request
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    request.Method = "GET";
                    var webResponse = request.GetResponse();
                    var webStream = webResponse.GetResponseStream();
                    var responseReader = new StreamReader(webStream);
                    string response = responseReader.ReadToEnd();
                    responseReader.Close();

                    //Parsing the Json
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic dobj = jsonSerializer.Deserialize<dynamic>(response);

                    //If Json is wrong, it wont have the parameter 'features'
                    result = dobj["features"][0].ToString();

                    JObject directionsJObject = JObject.Parse(result);
                    JToken coordinatesJToken = (JToken)directionsJObject.SelectToken("$.geometry.coordinates");
                    JArray coordinatesJArr = JArray.Parse(coordinatesJToken.ToString());
                    string[][] coordinatesArray = JsonConvert.DeserializeObject<string[][]>(coordinatesJArr.ToString());

                    //Initialize locations for polylines
                    Location location1 = new Location
                    {
                        Longitude = Convert.ToDouble(coordinatesArray[0][0]),
                        Latitude = Convert.ToDouble(coordinatesArray[0][1])
                    };
                    Location location2 = new Location();

                    //Drawing lines
                    for (int i = 1; i < coordinatesArray.Length; i++)
                    {
                        location2.Longitude = Convert.ToDouble(coordinatesArray[i][0]);
                        location2.Latitude = Convert.ToDouble(coordinatesArray[i][1]);
                        Xamarin.Forms.Maps.Polyline polyline = new Xamarin.Forms.Maps.Polyline
                        {
                            StrokeColor = Color.Blue,
                            StrokeWidth = 12,
                            Geopath =
                            {
                                new Position(location1.Latitude, location1.Longitude),
                                new Position(location2.Latitude, location2.Longitude)
                            }
                        };

                        MyMap.MapElements.Add(polyline);
                        location1.Longitude = location2.Longitude;
                        location1.Latitude = location2.Latitude;
                    }

                }
                catch (Exception e)
                {
                    await DisplayAlert("Alert", "Something went wrong.\nPlase try again", "Ok");
                    Console.WriteLine("Exception at getting coordinates / parsing json. Message: " + e.ToString());
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Alert", "No destination location", "Ok");
            }    
        }

    }
}

