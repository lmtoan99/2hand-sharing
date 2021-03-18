using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Message : BaseEntity
    {
        public string Content { get; set; }
        public int SendDate{get;set;}
        [ForeignKey("SendFromAccount")]
        public int SendFromAccountId { get; set; }
        [ForeignKey("SendToAccount")]
        public int SendToAccountId{get;set;}
        public Account SendFromAccount { get; set; }
        public Account SendToAccount { get; set; }
    }
}
