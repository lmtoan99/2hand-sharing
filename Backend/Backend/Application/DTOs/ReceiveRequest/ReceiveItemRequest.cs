using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Item
{
    public class ReceiveItemRequest
    {
        public int ItemId { get; set; }
        public string ReceiveReason { get; set; }
    }
}
