using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Message
{
    public class RecentMessagesDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
        public int SendFromAccountId { get; set; }
        public string SendFromAccountName { get; set; }
        public string AvatarUrlSendFromAccount { get; set; }
        public int SendToAccountId { get; set; }
        public string SendToAccountName { get; set; }
        public string AvatarUrlSendToAccount { get; set; }
    }
}
