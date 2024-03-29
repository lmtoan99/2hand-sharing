﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Settings
{
    public class MailSettings
    {
        public string EmailFrom { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
