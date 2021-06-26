using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Group : BaseEntity
    {
        public string GroupName{get;set;}
        public string Description{get;set;}
        public DateTime CreateDate{get;set;}
        public string Rules { get; set; }
        public int? AvatarId { get; set; }
        [ForeignKey("AvatarId")]
        [InverseProperty("GroupAvatar")]
        public virtual Image Avatar { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<GroupAdminDetail> GroupAdminDetails { get; set; }
        public virtual ICollection<GroupMemberDetail> GroupMemberDetails { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
    }
}
