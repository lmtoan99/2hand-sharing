using Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Item
{
    public class UpdateItemDTO
    {
        public string ItemName { get; set; }
        public AddressDTO ReceiveAddress { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int ImageNumber { get; set; }
        public List<string> DeletedImages { get; set; }
    }
}
