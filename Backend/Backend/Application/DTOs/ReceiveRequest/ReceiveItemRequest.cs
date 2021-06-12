using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Item
{
    public class ReceiveItemRequest
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string ReceiveReason { get; set; }
    }
}
