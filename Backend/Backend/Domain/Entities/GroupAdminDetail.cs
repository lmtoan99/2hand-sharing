using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class GroupAdminDetail : BaseEntity
    {
        public int AdminId { get; set; }
        public int GroupId{get;set;}
        public DateTime AppointDate{get;set;}
        [ForeignKey("AdminId")]
        [InverseProperty("GroupAdminDetails")]
        public virtual User Admin { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("GroupAdminDetails")]
        public virtual Group Group { get; set; }
    }
}
