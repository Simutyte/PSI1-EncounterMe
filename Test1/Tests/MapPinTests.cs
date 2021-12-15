// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe;
using Xunit;

namespace Test1.Tests
{
    public class MapPinTests
    {
        [Theory]
        [MemberData(nameof(TestData_CompareTo_Correct))]
        public void MapPin_CompareTo_Correct(object pin, string name, int expected)
        {
            MapPin testObj = new MapPin();
            testObj.Name = name;
            int result = testObj.CompareTo(pin);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestData_CompareTo_Correct()
        {
            //object, name, result
            yield return new object[] { null, "", 1};
            yield return new object[] { new MapPin { Name = "Name"}, "Name", 0};
            yield return new object[] { new MapPin { Name = "Test2"}, "Test2", 0};
            yield return new object[] { null, "Test2", 1};
        }

        [Theory]
        [MemberData(nameof(TestData_CompareTo_InCorrect))]
        public void MapPin_CompareTo_InCorrect(object pin, string name, int expected)
        {
            MapPin testObj = new MapPin();
            testObj.Name = name;

            if (pin == null)
            {
                int result = testObj.CompareTo(pin);
                Assert.NotEqual(expected, result);
            }
            else
            {
                Assert.Throws<ArgumentException>(() => testObj.CompareTo(pin));
            }
            
        }

        public static IEnumerable<object[]> TestData_CompareTo_InCorrect()
        {
            //pin == null
            yield return new object[] { null, "", 0 };
            yield return new object[] { null, "Test2", 2 };

            //Names 
            yield return new object[] { null, "Name1", 0 };

            //Exception (not pin)
            yield return new object[] { new Route { City = "Vilnius"} , "Test2", 0 };
            
        }

    }
}
