using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Item
{
    public class SendThanksDTO
    {
        [Required]
        public string thanks { get; set; }
    }
}
