using Application.Enums;
using Application.Features.ItemFeatures.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.ReceiveRequest
{
    public class ReceiveRequestDoneeViewModel
    {
        public int Id { get; set; }
        public string ReceiveReason { get; set; }
        public ReceiveItemInformationStatus ReceiveStatus { get; set; }
    }
}
