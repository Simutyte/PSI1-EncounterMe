using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncounterMe.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Pins;

namespace EncounterMe.Tests.Tests
{
    [TestClass()]
    public class MappinPinCoordinatesTests
    {
        [DataTestMethod]
        [DataRow(-90.01)]
        [DataRow(90.01)]
        public void AddPin_CheckLatitude_LatIsNotValid(double input)
        {
            bool isValid = AddPin.CheckLatitude(input);

            Assert.IsFalse(isValid);
        }

        [DataTestMethod]
        [DataRow(-180.01)]
        [DataRow(180.01)]
        public void AddPin_CheckLongitude_LongIsNotValid(double input)
        {
            bool isValid = AddPin.CheckLongitude(input);

            Assert.IsFalse(isValid);
        }
    }
}
