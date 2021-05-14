using Application.DTOs.Address;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ItemFeatures.Queries
{
    public class GetItemByIdViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public AddressDTO ReceiveAddress { get; set; }
        public DateTime PostTime { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrl { get; set; }
        public int DonateAccountId { get;set; }
        public string DonateAccountName { get; set; }
        public int UserRequestId { get; set; }
    }
}
