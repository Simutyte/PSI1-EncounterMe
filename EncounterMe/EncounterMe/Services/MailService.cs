// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using EncounterMe.Views;

namespace EncounterMe.Services
{
    class MailService
    {
        public MailService()
        {
      
        }

        public void OnSuccessfulRegistration(object source, RegistationEventArgs args)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("encounterMePSI@gmail.com");
            mail.To.Add(args.Email);
            mail.Subject = "Hello, its EncounterMe";
            mail.Body = "You have successfully registation on our EncounterMe app. Have a great time with us!";

            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("encounterMePSI@gmail.com", "EncounterMe2021");

            SmtpServer.Send(mail);
        }
    }
}
