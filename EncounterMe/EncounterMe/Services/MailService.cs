// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using EncounterMe.Views;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EncounterMe.Services
{
    class MailService
    {
        public MailService()
        {
      
        }

        public async void OnSuccessfulRegistration(object source, RegistationEventArgs args)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var sendGridClient = new SendGridClient(apiKey); //įrašyt key
            var from = new EmailAddress("EncounterMePSI@gmail.com", "Encounter Me");
            var to = new EmailAddress(args.Email);
            var id = "d-a94f5402bdc9411daba5266b1b56b297";
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, id, null);
            await sendGridClient.SendEmailAsync(msg);

            /*MailMessage mail = new MailMessage();
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

            SmtpServer.Send(mail);*/
        }
    }
}
