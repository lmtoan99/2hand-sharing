using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Firebase
{
    public class MessageNotiData
    {
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
        public int SendFromAccountId { get; set; }
        public string SendFromAccountName { get; set; }
    }
}
