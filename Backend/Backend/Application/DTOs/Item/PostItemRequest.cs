using Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Item
{
    public class PostItemRequest
    {
        [Required]
        public string ItemName { get; set; }
        [Required]
        public AddressDTO ReceiveAddress { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ImageNumber { get; set; }
    }
}
