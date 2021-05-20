using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Message
{
    public class SendMessageRequest
    {
        public string Content { get; set; }
        public int SendToAccountId { get; set; }
    }
}
