// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using EncounterMe.Services;
using Rg.Plugins.Popup.Services;
using EncounterMe.Views.Popups;
using System.Net;
using System.IO;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


//TODO: fix first polyline
//TODO: make route interactive
//TODO: 

namespace EncounterMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(Lat), "lat")]
    [QueryProperty(nameof(Longi), "longi")]
    [QueryProperty(nameof(DrawingRoute), "drawing")]
    [QueryProperty(nameof(DisplayPin), "pin")]
    [QueryProperty(nameof(SpecificRoute), "route")]
    public partial class MapPage : ContentPage
    {

        private Location _location = new Location();
        private Location _lastRegisteredLocation = new Location();

        private List<Polyline> _polylineList = new List<Polyline>();
        private PinsList _myPinList = null;
        private MapElement _firstPolyline = new MapElement();

        private string _routeType = "foot-walking";
        private double[][] _coordsArray;

        private bool _isTimerRunning = false;
        private bool _chosenLocationPermission;
        private bool _chosenGpsPermission;
        private double _averageDistance;
        private bool _destinationReached = false;
        private IEnumerable<MapPin> _specificRoutePins;


        public bool DisplayPin { get; set; } = false;
        public bool DrawingRoute { get; set; } = false;
        public bool SpecificRoute { get; set; } = false;
        public double Lat { get; set; } = 0;
        public double Longi { get; set; } = 0;

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
            GenerateDisplays();
        }

        public void GenerateDisplays()
        {
            //Checks if we need to display a pin(coming from "all objects -> add object -> by pin to maps") 
            if (DisplayPin)
            {
                AnimationView.IsVisible = !AnimationView.IsVisible;
                CenterPin.IsVisible = !CenterPin.IsVisible;
            }

            //Checks if we need to draw a route (coming form individual objects page)
            if (DrawingRoute && !SpecificRoute)
            {
                Location endLoc = new Location
                {
                    Latitude = Lat,
                    Longitude = Longi
                };

                DisplayRoute(endLoc);
            }

            //If it isnt a specific route, simply generate all pins
            if (!SpecificRoute)
            {
                GenerateMapPins();
            }
            else
            {
                _specificRoutePins = LoadRoutePage.PinsToRender;
                //MyMap.Pins.Clear();
                //MyMap.MapElements.Clear();
                foreach (var pin in _specificRoutePins)
                {
                    Pin pinToAdd = new Pin
                    {
                        Label = pin.Name,
                        Position = new Position(pin.Latitude, pin.Longitude)
                    };
                    MyMap.Pins.Add(pinToAdd);
                }

                if (DrawingRoute)
                {
                    var pin = _specificRoutePins.Where(x => x.Latitude != 0 && x.Longitude != 0 && !x.Visited).FirstOrDefault();
                    Lat = pin.Latitude;
                    Longi = pin.Longitude;
                    Location endLoc = new Location(Lat = pin.Latitude, Longi = pin.Longitude);
                    DisplayRoute(endLoc);
                    _destinationReached = false;
                }
            }
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DrawingRoute = false;
        }

        private async void DisplayCurrentLocation()
        {
            try
            {
                //Trying to display current location. GeolocationAccuracy.Default for quicker initialization
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                Location location = await Geolocation.GetLocationAsync(request);

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
            Location lastKnown = new Location();
            if (!_isTimerRunning)
            {
                _isTimerRunning = true;
                Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Default);
                        Location location = await Geolocation.GetLocationAsync(request);
                        //Checks if we are drawing route and if the location changed at all
                        if (DrawingRoute)
                        {
                            if (location.Latitude != lastKnown.Latitude && location.Longitude != lastKnown.Longitude)
                            {
                                UserLocationChanged(location);
                            }
                        }     
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
            Location startLocation = await Geolocation.GetLocationAsync(requestLocation);

            try
            {
                GetAndParseJson(startLocation, endLocation);
                DrawPolylines();
                RedrawFirstPolyline(startLocation);
            }
            catch (Exception e)
            {
                if (_routeType == "wheelchair")
                {
                    await DisplayAlert("Alert", "Couldn't find a path.\nPossibly because there isn't a way for wheelchair users", "Ok");
                }
                else
                {
                    await DisplayAlert("Alert", "Something went wrong.\nPlase try again", "Ok");
                    Console.WriteLine("Exception at getting coordinates / parsing json. Message: " + e.ToString());                    
                    await Navigation.PopAsync();
                }
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

            Console.WriteLine(URL);

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
            _coordsArray = JsonConvert.DeserializeObject<double[][]>(coordinatesJArr.ToString());

            //Recalculates the average distance after the array has changed
            CalculateAverage();
        }

        //Keep in mind, that this API uses format long:lat
        public void DrawPolylines()
        {
            //Clears previous polylines
            _polylineList.Clear();
            MyMap.MapElements.Clear();
            
            //Initiate end points of a polyline
            Location location2 = new Location();
            Location location1 = new Location
            {
                Longitude = _coordsArray[0][0],
                Latitude = _coordsArray[0][1]
            };
            
            //Drawing lines
            for (int i = 1; i < _coordsArray.Length; i++)
            {
                location2.Longitude = _coordsArray[i][0];
                location2.Latitude = _coordsArray[i][1];
                
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

                //Adding polyline to list and to map
                _polylineList.Add(polyline);
                MyMap.MapElements.Add(polyline);

                location1.Latitude = location2.Latitude;
                location1.Longitude = location2.Longitude;

            }
        }

        //Walking
        private void Button_Type_Foot(object sender, EventArgs e)   
        {
            Location loc = new Location
            {
                Latitude = Lat,
                Longitude = Longi
            };
            _routeType = "foot-walking";
            DisplayRoute(loc);
        }

        //Cycling
        private void Button_Type_Cycling(object sender, EventArgs e)
        {
            Location loc = new Location
            {
                Latitude = Lat,
                Longitude = Longi
            };
            _routeType = "cycling-regular";
            DisplayRoute(loc);
        }


        //Wheelchair
        private void Button_Type_Wheelchair(object sender, EventArgs e)
        {
            Location loc = new Location
            {
                Latitude = Lat,
                Longitude = Longi
            };
            _routeType = "wheelchair";
            DisplayRoute(loc);
        }

        //Driving
        private void Button_Type_Car(object sender, EventArgs e)
        {
            Location loc = new Location
            {
                Latitude = Lat,
                Longitude = Longi   
            };
            _routeType = "driving-car";
            DisplayRoute(loc);
        }


        //Draws polyline from current location to the beggining of route
        //could be better, if the line was dotted
        private async void UserLocationChanged(Location currentLocation)
        {
            double distanceToDestination = Location.CalculateDistance(Lat, Longi, currentLocation.Latitude, currentLocation.Longitude, DistanceUnits.Kilometers);

            //Paziuret kaip ten su trinimais route ir panasiai
            if (distanceToDestination < 0.02)
            {
                if (!_destinationReached)
                {
                    await DisplayAlert("Congratulations!", "Object added to visited objects list", "Ok");
                    _destinationReached = true;

                    if (SpecificRoute)
                    {
                        //_specificRoutePins = LoadRoutePage.PinsToRender;
                        foreach (var x in LoadRoutePage.PinsToRender)
                        {
                            //makes the closest pin visited
                            if (!x.Visited)
                            {
                                x.Visited = true;
                                _myPinList.ListOfPins.Find(pin => pin.Id == x.Id).Visited = true;
                                break;
                            }
                        }
                        _specificRoutePins = LoadRoutePage.PinsToRender;
                        CheckRoutes();
                    }
                    else
                    {
                        DrawingRoute = false;
                    }
                }
            }
            else
            {
                //Calculating the difference of last registered location and current location
                double distanceFromLastLocation = Location.CalculateDistance(_lastRegisteredLocation.Latitude, _lastRegisteredLocation.Longitude,
                                                                    currentLocation.Latitude, currentLocation.Longitude, DistanceUnits.Kilometers);

                //Calculates distance offset (used for further calculations)
                double distanceOffset = 0.1 * _averageDistance;     //TODO : check what works with offset percentage

                if (distanceFromLastLocation > 0 && _averageDistance > 0)
                {
                    if (distanceFromLastLocation < _averageDistance + distanceOffset)
                    {
                        _lastRegisteredLocation = currentLocation;
                        //RedrawFirstPolyline(currentLocation);
                    }
                    else
                    {
                        Location loc = new Location
                        {
                            Latitude = Lat,
                            Longitude = Longi
                        };
                        _lastRegisteredLocation = currentLocation;

                        DisplayRoute(loc);
                    }
                }
            }
        }

        public async void CheckRoutes()
        {
            if (SpecificRoute)
            {
                //check if its the last element
                List<MapPin> unvisitedPins = new List<MapPin>();
                foreach (var pin in _specificRoutePins)
                {
                    if (!pin.Visited)
                    {
                        unvisitedPins.Add(pin);
                    }
                }

                ClearMapElements();

                if (unvisitedPins.Count == 0)
                { 
                    await DisplayAlert("Congratulations!", "You finished a route", "Ok");
                    DrawingRoute = false;
                }
                else
                {
                    bool answer = await DisplayAlert("Alert", "Do you want to continue to the next location?", "Yes", "No");
                    if (answer)
                    {
                        GenerateDisplays();
                    }
                    else
                    {
                        DrawingRoute = false;
    
                    }
                }
            }
        }

        public void ClearMapElements()
        {
            _polylineList.Clear();
            MyMap.MapElements.Clear();
            _firstPolyline = null;
        }

        public void RedrawFirstPolyline(Location currentLocation)
        {
            if (_firstPolyline != null)
            {
                MyMap.MapElements.Remove(_firstPolyline);
            }

            Location firstPolyline= new Location
            {
                Latitude = _coordsArray[0][1],
                Longitude = _coordsArray[0][0]
            };

            Polyline polyline = new Polyline
            {

                StrokeColor = Color.LightBlue,
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

            for (int i = 0; i < _coordsArray.Length - 1; i++)
            {
                loc1.Latitude = _coordsArray[i][1];
                loc1.Longitude = _coordsArray[i][0];
                loc2.Latitude = _coordsArray[i + 1][1];
                loc2.Longitude = _coordsArray[i + 1][0];

                sum += Location.CalculateDistance(loc1.Latitude, loc1.Longitude, loc2.Latitude, loc2.Longitude, DistanceUnits.Kilometers);
            }
            
            _averageDistance = sum / (_coordsArray.Length - 1);

        }
    }
}


