using System;
using System.Collections.Generic;
using System.Text;
using NotificationService.Service.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using NotificationService.Service.Models;
using System.IO;

namespace NotificationService.Service.Helpers
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailsettings)
        {
            _mailSettings = mailsettings.Value;

        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            // email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.Sender = MailboxAddress.Parse("dotnetgroup06@gmail.com");
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            // email.To.Add(MailboxAddress.Parse("meharsh.gohadkar94@gmail.com"));
            email.Subject = mailRequest.Subject;
            // email.Subject = "Test Email Subject";

            var builder = new BodyBuilder();

            /*  if(mailRequest.Attachments !=null) 
              {
                  byte[] fileBytes;
                  foreach (var file in mailRequest.Attachments)
                  {
                      if(file.Length>0)
                      {
                          using (var ms = new MemoryStream())
                          {
                              file.CopyTo(ms);
                              fileBytes = ms.ToArray();
                          }
                         builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                      }
                  }
              }*/
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //  smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            smtp.Authenticate("dotnetgroup06@gmail.com", "Citiustech@123");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);


        }
    }
}
