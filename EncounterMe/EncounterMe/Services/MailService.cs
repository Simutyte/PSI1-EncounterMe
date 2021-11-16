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
            var sendGridClient = new SendGridClient("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"); //įrašyt key
            var from = new EmailAddress("EncounterMePSI@gmail.com", "Encounter Me");
            var to = new EmailAddress(args.Email);
            var id = "d-0ac86aa6b3df44ed927c08b99f84bd52";
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
