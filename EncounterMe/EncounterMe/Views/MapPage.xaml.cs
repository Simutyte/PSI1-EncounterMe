// Licensed to the .NET Foundation under one or more agreements.
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
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {

        private Location _location = new Location();

        private PinsList _myPinList;


        private bool _chosenLocationPermission;
        private bool _chosenGpsPermission;

        //public MapPage(Location loc = null)
        //{
        //    if (loc != null)
        //    {
        //        DisplayRoute(loc);
        //    }

        public MapPage()
        {

            _chosenLocationPermission = false;
            _chosenGpsPermission = false;

            InitializeComponent();
            DisplayCurrentLocation();


            Location location = new Location { Latitude = 38.01655470103673, Longitude = -121.88968844314147 };
            DisplayRoute(location);
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


            //URL to API
            string URL = $"http://api.openrouteservice.org/v2/directions/driving-car?" +
                $"api_key=5b3ce3597851110001cf62480ee65daaadbe486f9218ad7d5288ad0a" +
                $"&start={startLocation.Longitude},{startLocation.Latitude}" +
                $"&end={endLocation.Longitude},{endLocation.Latitude}";

            //Getting a request
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //request.Method = "GET";
            //var webResponse = request.GetResponse();
            //var webStream = webResponse.GetResponseStream();
            //var responseReader = new StreamReader(webStream);
            //string response = responseReader.ReadToEnd();
            //responseReader.Close();

            ////Parsing the Json
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //dynamic dobj = jsonSerializer.Deserialize<dynamic>(response);
            //string result = dobj["features"][0].ToString();


            string result = "{ \"bbox\":[8.681423,49.414599,8.69198,49.420514],\"type\":\"Feature\",\"properties\":{ \"segments\":[{ \"distance\":1603.8,\"duration\":265.9,\"steps\":[{ \"distance\":1.9,\"duration\":0.5,\"type\":11,\"instruction\":\"Head west on Gerhart-Hauptmann - Straße\",\"name\":\"Gerhart - Hauptmann - Straße\",\"way_points\":[0,1]},{ \"distance\":314,\"duration\":75.4,\"type\":3,\"instruction\":\"Turn sharp right onto Wielandtstraße\",\"name\":\"Wielandtstraße\",\"way_points\":[1,11]},{ \"distance\":737.6,\"duration\":106.2,\"type\":1,\"instruction\":\"Turn right onto Mönchhofstraße\",\"name\":\"Mönchhofstraße\",\"way_points\":[11,39]},{ \"distance\":264.3,\"duration\":38.5,\"type\":0,\"instruction\":\"Turn left onto Handschuhsheimer Landstraße, B 3\",\"name\":\"Handschuhsheimer Landstraße, B 3\",\"way_points\":[39,55]},{ \"distance\":155.3,\"duration\":14,\"type\":5,\"instruction\":\"Turn slight right onto Handschuhsheimer Landstraße, B 3\",\"name\":\"Handschuhsheimer Landstraße, B 3\",\"way_points\":[55,59]},{ \"distance\":130.7,\"duration\":31.4,\"type\":0,\"instruction\":\"Turn left onto Roonstraße\",\"name\":\"Roonstraße\",\"way_points\":[59,62]},{ \"distance\":0,\"duration\":0,\"type\":10,\"instruction\":\"Arrive at Roonstraße, straight ahead\",\"name\":\" - \",\"way_points\":[62,62]}]}],\"summary\":{ \"distance\":1603.8,\"duration\":265.9},\"way_points\":[0,62]},\"geometry\":{ \"coordinates\":[[8.681496,49.414601],[8.68147,49.414599],[8.681488,49.41465],[8.681423,49.415698],[8.681423,49.415746],[8.681427,49.415802],[8.681641,49.416539],[8.681656,49.41659],[8.681672,49.416646],[8.681825,49.417081],[8.681875,49.417287],[8.681881,49.417392],[8.682035,49.417389],[8.682107,49.41739],[8.682461,49.417389],[8.682563,49.417388],[8.682676,49.417387],[8.682781,49.417388],[8.683379,49.41738],[8.683595,49.417372],[8.683709,49.417368],[8.685294,49.417365],[8.685359,49.417365],[8.685442,49.417365],[8.685713,49.41737],[8.686407,49.417365],[8.686717,49.417366],[8.687376,49.417353],[8.687466,49.417351],[8.687547,49.417349],[8.688256,49.417361],[8.688802,49.417381],[8.690001,49.417413],[8.690275,49.417425],[8.690344,49.417424],[8.690434,49.417417],[8.691467,49.417155],[8.691735,49.417102],[8.691957,49.417117],[8.69198,49.417121],[8.691941,49.41722],[8.691817,49.417369],[8.691427,49.417726],[8.691072,49.418051],[8.690968,49.418158],[8.690936,49.418188],[8.690939,49.418208],[8.690939,49.418296],[8.69092,49.418378],[8.690912,49.418411],[8.69067,49.418981],[8.690664,49.418992],[8.690614,49.419093],[8.690547,49.419174],[8.690479,49.419204],[8.690446,49.419218],[8.690275,49.419577],[8.690123,49.419833],[8.689854,49.420216],[8.689652,49.420514],[8.68963,49.42051],[8.688601,49.420393],[8.687872,49.420318]],\"type\":\"LineString\"}}";


            JObject directionsJObject = JObject.Parse(result);
            JToken coordinatesJToken = (JToken)directionsJObject.SelectToken("$.geometry.coordinates");
            JArray coordinatesJArr = JArray.Parse(coordinatesJToken.ToString());

            string[][] coordinatesArray = JsonConvert.DeserializeObject <string[][]>(coordinatesJArr.ToString());


            //Initialize locations for polylines
            Location location1 = new Location{
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

    }
}

