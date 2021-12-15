﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe;
using EncounterMe.Views;
using Xamarin.Essentials;
using Xunit;

namespace Test1.Tests
{
    public class IndividualObjectsPageTests
    {

        [Theory]
        [InlineData(0.02)]
        [InlineData(0.01)]
        [InlineData(0)]
        public void IndividualObjectPage_Allow_True(double input)
        {
            bool allow = IndividualObjectPage.Allow(input);

            Assert.True(allow);
        }

        [Theory]
        [InlineData(0.021)]
        [InlineData(100)]
        public void IndividualObjectPage_Allow_False(double input)
        {
            bool allow = IndividualObjectPage.Allow(input);

            Assert.False(allow);
        }

        [Theory]
        [InlineData(20000, 0)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(20, 20)]
        public void IndividualObjectPage_IsClose_True(double input1, int input2)
        {
            bool isTrue = IndividualObjectPage.IsClose(input1, input2);

            Assert.True(isTrue);
        }

        [Theory]
        [InlineData(20000.1, 0)]
        [InlineData(20.1, 1)]
        [InlineData(100, 20)]
        public void IndividualObjectPage_IsClose_False(double input1, int input2)
        {
            bool isTrue = IndividualObjectPage.IsClose(input1, input2);

            Assert.False(isTrue);
        }

        [Theory]
        [InlineData(20000.1, 0)]
        [InlineData(100000, 0)]
        [InlineData(20.1, 1)]
        [InlineData(100, 20)]
        public void IndividualObjectPage_IsAway_True(double input1, int input2)
        {
            bool isTrue = IndividualObjectPage.IsAway(input1, input2);

            Assert.True(isTrue);
        }

        [Theory]
        [InlineData(20000, 0)]
        [InlineData(100000.1, 0)]
        [InlineData(20, 1)]
        [InlineData(100.1, 20)]
        public void IndividualObjectPage_IsAway_False(double input1, int input2)
        {
            bool isTrue = IndividualObjectPage.IsAway(input1, input2);

            Assert.False(isTrue);
        }

        [Theory]
        [InlineData(100000.1, 0)]
        [InlineData(100.1, 2)]
        public void IndividualObjectPage_IsFarAway_True(double input1, int input2)
        {
            bool isTrue = IndividualObjectPage.IsFarAway(input1, input2);

            Assert.True(isTrue);
        }

        [Theory]
        [InlineData(100000, 0)]
        [InlineData(100, 1)]
        public void IndividualObjectPage_IsFarAway_False(double input1, int input2)
        {
            bool isTrue = IndividualObjectPage.IsFarAway(input1, input2);

            Assert.False(isTrue);
        }

        [Theory]
        [MemberData(nameof(TestData_GetDistanceByIndex_Correct))]
        public void IndividualObjectPage_GetDistanceByIndex_DistanceCorrect(Location loc, MapPin pin, int i, double expected)
        {
            double? result = IndividualObjectPage.GetDistanceByIndex(i, loc, pin);

            if (i < 0 || i > 3 )
            {
                Assert.Null(result);
            }
            else
            {
                Assert.Equal(expected, Math.Round(result.Value, 2));
            }
        }

        public static IEnumerable<object[]> TestData_GetDistanceByIndex_Correct()
        {
            //Meters
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       0,
                                       Math.Round(157.249381 * 1000, 2)};

            //Km
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       1,
                                       Math.Round(157.249381, 2)};

            //Yards
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       2,
                                       Math.Round(97.71023535 * 1760, 2)};

            //Miles
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       3,
                                       Math.Round(97.71023535, 2)};
            //for null
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       4,
                                       Math.Round(97.71023535, 2)};
        }

        [Theory]
        [MemberData(nameof(TestData_GetDistanceByIndex_Incorrect))]
        public void IndividualObjectPage_GetDistanceByIndex_DistanceIncorrect(Location loc, MapPin pin, int i, double expected)
        {
            double? result = IndividualObjectPage.GetDistanceByIndex(i, loc, pin);

            if (i < 0 || i > 3)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotEqual(expected, Math.Round(result.Value, 2));
            }
        }

        public static IEnumerable<object[]> TestData_GetDistanceByIndex_Incorrect()
        {
            //Meters
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       0,
                                       Math.Round(157.2 * 1000, 2)};

            //Km
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       1,
                                       Math.Round(157.2, 2)};

            //Yards
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       2,
                                       Math.Round(97.7 * 1760, 2)};

            //Miles
            yield return new object[] {new Location { Latitude = 0,
                                                      Longitude = 0},
                                       new MapPin{    Latitude = 1,
                                                      Longitude = 1, },
                                       3,
                                       Math.Round(97.7, 2)};
        }
    }
}