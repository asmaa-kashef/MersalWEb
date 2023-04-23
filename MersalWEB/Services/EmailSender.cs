using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace MersalWEB.Services
{
    
        public class EmailSender : IEmailSender
        {


            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var fromMail = "asmaa.ali.kashef@gmail.com";
                var fromPassword = "trcejhtyrowwdqsa";

                var message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = subject;
                message.To.Add(email);
                message.Body = $"<html><body>{htmlMessage}</body></html>";
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("Smtp.Gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(message);
            }
        }
        //Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    throw new NotImplementedException();
        //}


    }

