using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Assignment : BaseEntity
    {
        public DateTime AssignmentDate{get;set;}
        public DateTime ExpirationDate { get; set; }
        public int Status{get;set;}
        public string Note{get;set;}
        public int DonateEventInformationId{get;set;}
        [ForeignKey("AssignedMemberAccount")]
        public int AssignedMemberId{get;set;}
        [ForeignKey("AssignByAccount")]
        public int AssignByAccountId { get;set;}
        public Account AssignByAccount { get; set; }
        public Account AssignedMember { get; set; }
    }
}
