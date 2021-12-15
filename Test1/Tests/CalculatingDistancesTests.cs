// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe;
using EncounterMe.Pins;
using Xamarin.Essentials;
using Xunit;

namespace Test1.Tests
{
    public class CalculatingDistancesTests
    {

        //Tests for calculating in km
        [Theory]
        [MemberData(nameof(TestData_GetDistanceInKm_Correct))]
        public void CalculatingDistances_GetDistanceInKm_Correct(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInKm(loc, pin);
            Assert.Equal(expected, Math.Round(returnDistance, 3));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInKm_Correct()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(157.249381, 3)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(12389.048054, 3)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(6820.144949, 3)};
        }

        [Theory]
        [MemberData(nameof(TestData_GetDistanceInKm_Incorrect))]
        public void CalculatingDistances_GetDistanceInKm_Incorrect(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInKm(loc, pin);
            Assert.NotEqual(expected, Math.Round(returnDistance, 3));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInKm_Incorrect()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(100.0, 3)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(200.1, 3)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(300.0, 3)};
        }

        //Tests for calculating in miles
        [Theory]
        [MemberData(nameof(TestData_GetDistanceInMiles_Correct))]
        public void CalculatingDistances_GetDistanceInMiles_Correct(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInMiles(loc, pin);
            Assert.Equal(expected, Math.Round(returnDistance, 3));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInMiles_Correct()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(97.71023535, 3)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(7698.19756, 3)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(4237.8415982, 3)};
        }

        [Theory]
        [MemberData(nameof(TestData_GetDistanceInMiles_Incorrect))]
        public void CalculatingDistances_GetDistanceInMiles_Incorrect(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInMiles(loc, pin);
            Assert.NotEqual(expected, Math.Round(returnDistance, 3));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInMiles_Incorrect()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(100.0, 3)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(200.1, 3)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(300.0, 3)};
        }

        //Tests for calculating in meters
        [Theory]
        [MemberData(nameof(TestData_GetDistanceInMeters_Correct))]
        public void CalculatingDistances_GetDistanceInMeters_Correct(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInMeters(loc, pin);
            Assert.Equal(expected, Math.Round(returnDistance, 3));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInMeters_Correct()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(157.249381 * 1000, 3)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(12389.048054 * 1000, 3)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(6820.144949 * 1000, 3)};
        }

        [Theory]
        [MemberData(nameof(TestData_GetDistanceInMeters_Incorrect))]
        public void CalculatingDistances_GetDistanceInMeters_Incorrect(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInMeters(loc, pin);
            Assert.NotEqual(expected, Math.Round(returnDistance, 3));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInMeters_Incorrect()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(100.0 * 1000, 3)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(200.1 * 1000, 3)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(300.0 * 1000, 3)};
        }

        //Tests for calculating in yards
        [Theory]
        [MemberData(nameof(TestData_GetDistanceInYards_Correct))]
        public void CalculatingDistances_GetDistanceInYards_Correct(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInYards(loc, pin);
            Assert.Equal(expected, Math.Round(returnDistance, 2));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInYards_Correct()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(97.71023535 * 1760, 2)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(7698.19756 * 1760, 2)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(4237.8415982 * 1760, 2)};
        }

        [Theory]
        [MemberData(nameof(TestData_GetDistanceInYards_Incorrect))]
        public void CalculatingDistances_GetDistanceInYards_Incorrect(Location loc, MapPin pin, double expected)
        {
            double returnDistance = Calculating.GetDistanceInYards(loc, pin);
            Assert.NotEqual(expected, Math.Round(returnDistance, 2));
        }

        public static IEnumerable<object[]> TestData_GetDistanceInYards_Incorrect()
        {
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       Math.Round(100.0 * 1760, 2)};

            yield return new object[] {new Location { Latitude = 100,
                                                      Longitude = 100},
                                       new MapPin{    Latitude = 2000,
                                                      Longitude = 2000, },
                                       Math.Round(200.1 * 1760, 2)};

            yield return new object[] {new Location { Latitude = -50,
                                                      Longitude = -60},
                                       new MapPin{    Latitude = -150,
                                                      Longitude = -160, },
                                       Math.Round(300.0 * 1760, 2)};
        }

    }
}
