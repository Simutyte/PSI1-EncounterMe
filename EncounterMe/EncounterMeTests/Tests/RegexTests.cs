// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using EncounterMe.Pins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncounterMeTests.Tests
{
    [TestClass]
    public class RegexTests
    {
        [DataTestMethod]
        [DataRow("123")]
        [DataRow("Abcdefgh@")]
        public void RegexDelegates_CheckEmail_EmailIsNotValid(string input)
        {
            bool isValid = RegexDelegates.CheckEmail(input);
            Assert.IsFalse(isValid);
        }

        [DataTestMethod]
        [DataRow("Abcde@gmail.com")]
        [DataRow("abcdefgh@yahoo.com")]
        public void RegexDelegates_CheckEmail_EmailIsValid(string input)
        {
            bool isValid = RegexDelegates.CheckEmail(input);
            Assert.IsTrue(isValid);
        }

        [DataTestMethod]
        [DataRow("Labas", "Labas")]
        [DataRow("acDeG25.a", "acDeG25.a")]
        public void RegexDelegates_CheckPasswordsMatch_PasswordsMatch(string pass1, string pass2)
        {
            bool isValid = RegexDelegates.CheckPasswordsMatch(pass1, pass2);
            Assert.IsTrue(isValid);
        }

        [DataTestMethod]
        [DataRow("Labas", "labas")]
        [DataRow("acDeG25.a", "acDEG25.a")]
        public void RegexDelegates_CheckPasswordsMatch_PasswordsDontMatch(string pass1, string pass2)
        {
            bool isValid = RegexDelegates.CheckPasswordsMatch(pass1, pass2);
            Assert.IsFalse(isValid);
        }

        [DataTestMethod]
        [DataRow("Labas", "Labas")]
        [DataRow("Labas555", "Labas555")]
        [DataRow("abcdes.5", "abcdes.5")]
        [DataRow("LABAS555", "LABAS555")]
        public void RegexDelegates_CheckPasswords_PasswordsNotAsRequired(string pass1, string pass2)
        {
            bool isValid = RegexDelegates.CheckPasswords(pass1, pass2);
            Assert.IsFalse(isValid);
        }

        [DataTestMethod]
        [DataRow("Labass.0", "Labass.0")]
        [DataRow("penkiG?555", "penkiG?555")]
        public void RegexDelegates_CheckPasswords_PasswordsAsRequired(string pass1, string pass2)
        {
            bool isValid = RegexDelegates.CheckPasswords(pass1, pass2);
            Assert.IsTrue(isValid);
        }

    }
}
