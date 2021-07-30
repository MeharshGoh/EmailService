using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NotificationService.Service.Models;

namespace NotificationService.Service.Contracts
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
    }
}
