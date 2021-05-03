using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
