using Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Item
{
    public class GetListMyItemDonateViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public AddressDTO Address { get; set; }
        public DateTime PostTime { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
