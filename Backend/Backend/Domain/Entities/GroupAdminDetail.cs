using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class GroupAdminDetail : BaseEntity
    {
        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        [ForeignKey("Group")]
        public int GroupId{get;set;}
        public DateTime AppointDate{get;set;}
        public Account Admin { get; set; }
        public Group Group { get; set; }
    }
}
