using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Dozentenplanung
{
    public class MailHelper
    {
        //Static
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static string Sender { get; set; }
        public static bool EnableSsl { get; set; } 
        public static string Username { get; set; }
        public static string Password { get; set; }

        public static void Initialize(IServiceProvider serviceProvider) {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var theDatabaseContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                /*MailHelper.Host = theDatabaseContext.settingForName("MailHost").Value;
                MailHelper.Username = theDatabaseContext.settingForName("MailUsername").Value;
                MailHelper.Password = theDatabaseContext.settingForName("MailPasswords").Value;*/
                MailHelper.Host = "localhost";
                MailHelper.Username = "";
                MailHelper.Password = "";
                MailHelper.Port = 2500;
                MailHelper.EnableSsl = false;
                MailHelper.Sender = "test";
            }
        }

        public static void SendTestMail()
        {
            MailHelper theHelper = new MailHelper
            {
                RecipientAddress = "simsaschneider@icloud.com",
                SenderAddress = "simsaschneider@icloud.com",
                Subject = "Test E-Mail",
                Content = "Das ist ein Test"
            };
            theHelper.Send();
        }



        //Instance
        public string RecipientAddress { get; set; }
        public string SenderAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public MailHelper() {
            this.RecipientAddress = "";
            this.Content = "";
        }

        public MailHelper(string recipient, string subject, string content) {
            this.RecipientAddress = recipient;
            this.Content = content;
            this.Subject = subject;
            this.SenderAddress = MailHelper.Sender;
        }

        public void Send() {
            SmtpClient theSmtpClient = new SmtpClient();

            theSmtpClient.Host = MailHelper.Host;
            theSmtpClient.Port = MailHelper.Port;
            theSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            theSmtpClient.UseDefaultCredentials = false;
            //theSmtpClient.Credentials = new NetworkCredential(MailHelper.Username, MailHelper.Password);
            theSmtpClient.EnableSsl = MailHelper.EnableSsl;

            MailMessage theMailMessage = new MailMessage(this.SenderAddress, this.RecipientAddress);
            theMailMessage.BodyEncoding = Encoding.UTF8;
            theMailMessage.Subject = this.Subject;
            theMailMessage.Body = this.Content;

            theSmtpClient.Send(theMailMessage);
        }
    }
}
