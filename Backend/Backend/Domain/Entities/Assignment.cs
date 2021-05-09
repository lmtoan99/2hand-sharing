using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public DateTime AssignmentDate{get;set;}
        public DateTime ExpirationDate { get; set; }
        public int Status{get;set;}
        public string Note{get;set;}
        public int DonateEventInformationId{get;set;}
        public int AssignedMemberId{get;set;}
        public int AssignByAccountId { get;set;}
        [ForeignKey("AssignByAccountId")]
        [InverseProperty("AdminAssigns")]
        public virtual User AssignByAccount { get; set; }
        [ForeignKey("AssignedMemberId")]
        [InverseProperty("Assignments")]
        public virtual User AssignedMember { get; set; }
        [ForeignKey("DonateEventInformationId")]
        [InverseProperty("Assignments")]
        public virtual DonateEventInformation DonateEventInformation { get; set; }
    }
}
