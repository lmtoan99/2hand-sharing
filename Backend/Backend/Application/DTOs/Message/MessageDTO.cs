using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Message
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
        public int SendFromAccountId { get; set; }
        public int SendToAccountId { get; set; }
    }
}
