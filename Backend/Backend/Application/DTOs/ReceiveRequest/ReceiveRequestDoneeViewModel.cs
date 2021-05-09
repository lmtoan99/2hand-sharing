using Application.Features.ItemFeatures.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.ReceiveRequest
{
    public class ReceiveRequestDoneeViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemImageUrl { get; set; }
    }
}
