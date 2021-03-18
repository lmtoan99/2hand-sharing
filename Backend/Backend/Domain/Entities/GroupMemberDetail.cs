using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class GroupMemberDetail : BaseEntity
    {
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        [ForeignKey("Group")]
        public int GroupId{get;set;}
        public bool ReportStatus{get;set;}
        public DateTime JoinDate{get;set;}
        public Account Member { get; set; }
        public Group Group { get; set; }
    }
}
