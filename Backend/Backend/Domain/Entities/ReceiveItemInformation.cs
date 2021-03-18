using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class ReceiveItemInformation : BaseEntity
    {
        public int ReceiveStatus { get; set; }
        public string Thanks { get; set; }
        public string ReceiveReason { get; set; }
        [ForeignKey("DonatePostInformation")]
        public int DonatePostInformationId { get; set; }
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public DonatePostInformation DonatePostInformation { get; set; }
        public Account Receiver { get; set; }
    }
}
