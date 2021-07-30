using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Service.Models
{
    public class WelcomeRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
    }
}
