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
    public class RegexTests
    {
        [Theory]
        [InlineData("123")]
        [InlineData("Abcdefgh@")]
        public void RegexDelegates_CheckEmail_EmailIsNotValid(string input)
        {
            bool isValid = RegexValidations.CheckEmail(input);
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Abcde@gmail.com")]
        [InlineData("abcdefgh@yahoo.com")]
        public void RegexDelegates_CheckEmail_EmailIsValid(string input)
        {
            bool isValid = RegexValidations.CheckEmail(input);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("Labas", "Labas")]
        [InlineData("acDeG25.a", "acDeG25.a")]
        public void RegexDelegates_CheckPasswordsMatch_PasswordsMatch(string pass1, string pass2)
        {
            bool isValid = RegexValidations.CheckPasswordsMatch(pass1, pass2);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("Labas", "labas")]
        [InlineData("acDeG25.a", "acDEG25.a")]
        public void RegexDelegates_CheckPasswordsMatch_PasswordsDontMatch(string pass1, string pass2)
        {
            bool isValid = RegexValidations.CheckPasswordsMatch(pass1, pass2);
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Labas", "Labas")]
        [InlineData("Labas555", "Labas555")]
        [InlineData("abcdes.5", "abcdes.5")]
        [InlineData("LABAS555", "LABAS555")]
        public void RegexDelegates_CheckPasswords_PasswordsNotAsRequired(string pass1, string pass2)
        {
            bool isValid = RegexValidations.CheckPasswords(pass1, pass2);
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Labass.0", "Labass.0")]
        [InlineData("penkiG?555", "penkiG?555")]
        public void RegexDelegates_CheckPasswords_PasswordsAsRequired(string pass1, string pass2)
        {
            bool isValid = RegexValidations.CheckPasswords(pass1, pass2);
            Assert.True(isValid);
        }
    }
}
