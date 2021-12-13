// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EncounterMe.Pins
{
    public class RegexDelegates
    {
        public static bool CheckEmail(string text)
        {
            var emailPatter = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            return Regex.IsMatch(text, emailPatter);
        }

        public static bool CheckPasswords(string password, string passwordConfirm)
        {

            //declaring regex for different passes
            Regex isUpperCase = new Regex(@"[A-Z]+");
            Regex isLowerCase = new Regex(@"[a-z]+");
            Regex isDigit = new Regex(@"[0-9]+");
            Regex isSymbol = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            Regex isLenght = new Regex(@".{8,20}");

            //checking if password passes these checks
            bool checkUpper = isUpperCase.IsMatch(password);
            bool checkLower = isLowerCase.IsMatch(password);
            bool checkDigit = isDigit.IsMatch(password);
            bool checkSymbol = isSymbol.IsMatch(password);
            bool checkLenght = isLenght.IsMatch(password);

            Func<string, string, bool> obj = new Func<string, string, bool>(CheckPasswordsMatch);
            bool passwordsMatch = obj.Invoke(password, passwordConfirm);

            //bool passwordsMatch = string.Equals(entryRegPassword.Text, entryRegPasswordConfirm.Text);

            bool passed = false;

            if (!passwordsMatch)
            {
                passed = false;
            }
            else if (!checkUpper)
            {
                passed = false;
            }
            else if (!checkLower)
            {
                passed = false;
            }
            else if (!checkDigit)
            {
                passed = false;
            }
            else if (!checkSymbol)
            {
                passed = false;
            }
            else if (!checkLenght)
            {
                passed = false;
            }
            else
            {
                //passed all checks
                passed = true;
            }

            //didnt pass something
            return passed;
        }

        public static bool CheckPasswordsMatch(string pass1, string pass2)
        {
            return string.Equals(pass1, pass2) ? true : false;
        }
    }
}

