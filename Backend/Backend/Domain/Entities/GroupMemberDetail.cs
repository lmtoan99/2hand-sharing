using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class GroupMemberDetail : BaseEntity
    {
        public int MemberId { get; set; }
        public int GroupId{get;set;}
        public bool ReportStatus{get;set;}
        public DateTime JoinDate{get;set;}
        [ForeignKey("MemberId")]
        [InverseProperty("GroupMemberDetails")]
        public virtual Account Member { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("GroupMemberDetails")]
        public virtual Group Group { get; set; }
    }
}
