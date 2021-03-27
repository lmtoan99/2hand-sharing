using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Item
{
    public class PostItemRequest
    {
        public string ItemName { get; set; }
        public string ReceiveAddress { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int ImageNumber { get; set; }
    }
}
