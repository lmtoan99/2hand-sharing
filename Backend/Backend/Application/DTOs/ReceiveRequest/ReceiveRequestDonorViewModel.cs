﻿using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Item
{
    public class ReceiveRequestDonorViewModel
    {
        public int Id { get; set; }
        public string ReceiveReason { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public ReceiveItemInformationStatus ReceiveStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string AvatarUrl { get; set; }
    }
}
