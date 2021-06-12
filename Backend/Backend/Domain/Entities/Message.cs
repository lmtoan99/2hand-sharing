using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Content { get; set; }
        public DateTime SendDate{get;set;}
        public int SendFromAccountId { get; set; }
        public int SendToAccountId{get;set;}
        [ForeignKey("SendFromAccountId")]
        [InverseProperty("MessageSends")]
        public virtual User SendFromAccount { get; set; }
        [ForeignKey("SendToAccountId")]
        [InverseProperty("MessageReceives")]
        public virtual User SendToAccount { get; set; }
    }
}
