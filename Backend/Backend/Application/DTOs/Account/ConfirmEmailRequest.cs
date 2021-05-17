using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Account
{
    public class ConfirmEmailRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
