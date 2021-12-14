// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Pins;
using Xunit;

namespace Test1.Tests
{
    public class MappinPinCoordinatesTests
    {
        [Theory]
        [InlineData(-90.01)]
        [InlineData(90.01)]
        public void AddPin_CheckLatitude_LatIsNotValid(double input)
        {
            bool isValid = AddPin.CheckLatitude(input);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(-180.01)]
        [InlineData(180.01)]
        public void AddPin_CheckLongitude_LongIsNotValid(double input)
        {
            bool isValid = AddPin.CheckLongitude(input);

            Assert.False(isValid);
        }
    
    }
}
