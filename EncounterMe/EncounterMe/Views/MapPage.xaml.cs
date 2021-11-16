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
using System.Collections.Generic;



//TODO: fix first polyline
//TODO: make route interactive
//TODO: 

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(Lat), "lat")]
    [QueryProperty(nameof(Longi), "longi")]
    [QueryProperty(nameof(DrawingRoute), "drawing")]
    public partial class MapPage : ContentPage
    {
        public event Action<Location> UserLocationChangedEvent;

        private Location _location = new Location();
        private Location _lastRegisteredLocation = new Location();

        private List<Polyline> _polylineList = new List<Polyline>();
        private PinsList _myPinList = null;
        private MapElement _firstPolyline = new MapElement();

        private string _routeType = "foot-walking";
        private string[][] _coordinatesArray;
        
        private bool _isTimerRunning = false;
        private bool _isDrawingRoute = false;
        private bool _chosenLocationPermission;
        private bool _chosenGpsPermission;

        private double _averageDistance;
        private double lat = 0;
        private double longi = 0;
        

        public bool DrawingRoute
        {
            get
            {
                return _isDrawingRoute;
            }
            set
            {
                _isDrawingRoute = value;
            }
        }
        public double Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }

        public double Longi
        {
            get
            {
                return longi;
            }
            set
            {
                longi = value;
            }
        }

        public MapPage()
        {
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


            if (_isDrawingRoute)
            {
                UserLocationChangedEvent += new Action<Location>(UserLocationChangedEventHandler);
                Location endLoc = new Location
                {
                    Latitude = lat,
                    Longitude = longi
                };

                DisplayRoute(endLoc);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _isDrawingRoute = false;
        }
        


        public void GenerateMapPins()
        {
            PinsList pinsList = PinsList.GetPinsList();
            _myPinList = pinsList;

            foreach (MapPin mapPin in _myPinList.ListOfPins.ToArray())
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
                if (!_chosenGpsPermission)
                {
                    PromptToEnableGPS();
                }
            }
            catch (PermissionException)
            {
                //If location permission is not already enabled and not yet chosen, prompt the user and call the method once again
                if (!_chosenLocationPermission)
                {
                    PromptToEnableLocationPermission();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in DisplayCurrentLocation. Message: " + e.ToString());
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
                IGpsSettings GpsDependency = DependencyService.Get<IGpsSettings>();         //Referencing the interface
                GpsDependency.OpenSettings();
                DisplayCurrentLocation();
            }
        }


        //Constantly runs in the background, tracks current location and updates it
        private void TrackingLiveLocation()
        {
            if (!_isTimerRunning)
            {
                _isTimerRunning = true;
                Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(2000);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var request = new GeolocationRequest(GeolocationAccuracy.Default);
                            var location = await Geolocation.GetLocationAsync(request);
                            if (_isDrawingRoute)
                            {
                                UserLocationChangedEvent(location);
                            }     
                        });
                    });
                    return true;
                });
            }
        }

        void UpdateCurrentLocation(Location loc)
        {
            Position p = new Position(loc.Latitude, loc.Longitude);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(1));
            MyMap.MoveToRegion(mapSpan);
        }

        void Add_Pin_Button_Clicked(object sender, EventArgs e)
        {
            AnimationView.IsVisible = !AnimationView.IsVisible;
            CenterPin.IsVisible = !CenterPin.IsVisible;
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


        public async void DisplayRoute(Location endLocation)
        {
            //Showing types button
            RouteTypes.IsVisible = true;

            //Requesting current location
            var requestLocation = new GeolocationRequest(GeolocationAccuracy.Default);
            var startLocation = await Geolocation.GetLocationAsync(requestLocation);

            try
            {
                GetAndParseJson(startLocation, endLocation);
                //Thread.Sleep(1000);
                DrawPolylines();
            }
            catch (Exception e)
            {
                await DisplayAlert("Alert", "Something went wrong.\nPlase try again", "Ok");
                Console.WriteLine("Exception at getting coordinates / parsing json. Message: " + e.ToString());
                await Navigation.PopAsync();
            }
        }

        //The current logic is that it won't parse a different message than defined, therefore causing an exception
        public void GetAndParseJson(Location startLocation, Location endLocation)
        {

            //URL to API
            string URL = $"http://api.openrouteservice.org/v2/directions/" +
            $"{_routeType}?api_key=5b3ce3597851110001cf62480ee65daaadbe486f9218ad7d5288ad0a" +
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

            //If Json is wrong, it wont have the parameter 'features' -- extend Exceptions
            string result = dobj["features"][0].ToString();

            JObject directionsJObject = JObject.Parse(result);
            JToken coordinatesJToken = directionsJObject.SelectToken("$.geometry.coordinates");
            JArray coordinatesJArr = JArray.Parse(coordinatesJToken.ToString());
            _coordinatesArray = JsonConvert.DeserializeObject<string[][]>(coordinatesJArr.ToString());

            //Recalculates the average distance after the array has changed
            CalculateAverage();
        }

        //Keep in mind, that this API uses format long:lat
        public void DrawPolylines()
        {
            //Clears previous polylines
            MyMap.MapElements.Clear();
            
            Location location2 = new Location();
            Location location1 = new Location
            {
                Longitude = Convert.ToDouble(_coordinatesArray[0][0]),
                Latitude = Convert.ToDouble(_coordinatesArray[0][1])
            };

            string[][] arr = _coordinatesArray;

            //Drawing lines
            for (int i = 0; i < arr.Length; i++)
            {
                location2.Longitude = Convert.ToDouble(arr[i][0]);
                location2.Latitude = Convert.ToDouble(arr[i][1]);
                Polyline polyline = new Polyline
                {
                    StrokeColor = Color.Blue,
                    StrokeWidth = 12,
                    Geopath =
                {
                    new Position(location1.Latitude, location1.Longitude),
                    new Position(location2.Latitude, location2.Longitude)
                }
                };
                _polylineList.Add(polyline);
                MyMap.MapElements.Add(polyline);

                location1.Longitude = location2.Longitude;
                location1.Latitude = location2.Latitude;
            }
        }


        //Walk
        private void Button_Type_Foot(object sender, EventArgs e)   
        {
            Location loc = new Location
            {
                Latitude = lat,
                Longitude = longi
            };
            _routeType = "foot-walking";
            DisplayRoute(loc);
        }

        //Bike
        private void Button_Type_Cycling(object sender, EventArgs e)
        {
            Location loc = new Location
            {
                Latitude = lat,
                Longitude = longi
            };
            _routeType = "cycling-regular";
            DisplayRoute(loc);
        }


        //Wheelchair
        private void Button_Type_Wheelchair(object sender, EventArgs e)
        {
            Location loc = new Location
            {
                Latitude = lat,
                Longitude = longi
            };
            _routeType = "wheelchair";
            DisplayRoute(loc);
        }

        private void Button_Type_Car(object sender, EventArgs e)
        {
            Location loc = new Location
            {
                Latitude = lat,
                Longitude = longi   
            };
            _routeType = "driving-car";
            DisplayRoute(loc);
        }


        //Draws polyline from current location to the beggining of route
        //could be better, if the line was dotted
        private void UserLocationChangedEventHandler(Location currentLocation)
        {
            double distance = Location.CalculateDistance(_lastRegisteredLocation.Latitude, _lastRegisteredLocation.Longitude,
                                                                currentLocation.Latitude, currentLocation.Longitude, DistanceUnits.Kilometers);

            //Calculates offset
            double distanceOffset = 0.1 * _averageDistance;     //TODO : check what works with offset percentage

            if (distance != 0 && _averageDistance != 0)
            {
                if (distance < _averageDistance + distanceOffset)
                {
                    _lastRegisteredLocation = currentLocation;
                    RedrawFirstPolyline(currentLocation);
                }
                else
                {
                    Location loc = new Location
                    {
                        Latitude = lat,
                        Longitude = longi
                    };
                    _lastRegisteredLocation = currentLocation;

                    DisplayRoute(loc);
                }
            }
        }

        public void RedrawFirstPolyline(Location currentLocation)
        {
            if (_firstPolyline != null)
            {
                MyMap.MapElements.Remove(_firstPolyline);
            }

            //DrawPolylines();
            Location firstPolyline = new Location
            {
                Latitude = Convert.ToDouble(_coordinatesArray[0][1]),
                Longitude = Convert.ToDouble(_coordinatesArray[0][0])
            };

            Polyline polyline = new Polyline
            {

                StrokeColor = Color.Blue,
                StrokeWidth = 12,
                Geopath =
            {
                new Position(currentLocation.Latitude, currentLocation.Longitude),
                new Position(firstPolyline.Latitude, firstPolyline.Longitude)
            }
            };

            MyMap.MapElements.Add(polyline);
            _firstPolyline = polyline;

        }

        public void CalculateAverage()
        {
            double sum = 0;
            Location loc1 = new Location();
            Location loc2 = new Location();

            for (int i = 0; i < _coordinatesArray.Length - 1; i++)
            {
                loc1.Latitude = Convert.ToDouble(_coordinatesArray[i][1]);
                loc1.Longitude = Convert.ToDouble(_coordinatesArray[i][0]);
                loc2.Latitude = Convert.ToDouble(_coordinatesArray[i + 1][1]);
                loc2.Longitude = Convert.ToDouble(_coordinatesArray[i + 1][0]);

                sum += Location.CalculateDistance(loc1.Latitude, loc1.Longitude, loc2.Latitude, loc2.Longitude, DistanceUnits.Kilometers);
            }
            
            _averageDistance = sum / (_coordinatesArray.Length - 1);

        }
    }
}


