using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ReceiveItemInformation : BaseEntity
    {
        public int ReceiveStatus { get; set; }
        public string Thanks { get; set; }
        public string ReceiveReason { get; set; }
        public int ItemId { get; set; }
        public int ReceiverId { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("ReceiveItemInformations")]
        public virtual Item Items { get; set; }
        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceiveItemInformations")]
        public virtual Account Receiver { get; set; }
    }
}
