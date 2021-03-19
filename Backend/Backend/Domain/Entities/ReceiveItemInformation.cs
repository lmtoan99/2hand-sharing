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
        public int DonatePostInformationId { get; set; }
        public int ReceiverId { get; set; }
        [ForeignKey("DonatePostInformationId")]
        [InverseProperty("ReceiveItemInformations")]
        public virtual DonatePostInformation DonatePostInformation { get; set; }
        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceiveItemInformations")]
        public virtual Account Receiver { get; set; }
    }
}
