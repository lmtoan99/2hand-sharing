﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Item
{
    public class ReceiveRequestViewModel
    {
        public string ReceiveReason { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
    }
}
