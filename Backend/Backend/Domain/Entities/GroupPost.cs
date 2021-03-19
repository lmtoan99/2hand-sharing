using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class GroupPost : BaseEntity
    {
        public string Content { get; set; }
        public DateTime PostTime{get;set;}
        public int GroupId{get;set;}
        public int PostByAccountId{get;set;}
        public int Visibility{get;set;}
        [ForeignKey("GroupId")]
        [InverseProperty("GroupPosts")]
        public virtual Group Group { get; set; }
        [ForeignKey("PostByAccountId")]
        [InverseProperty("GroupPosts")]
        public virtual Account PostByAccount { get; set; }
        public virtual ICollection<GroupPostImageRelationship> GroupPostImageRelationships { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
