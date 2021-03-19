using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class GroupPostImageRelationship : BaseEntity
    {
        public int ImageId { get; set; }
        public int PostId{get;set;}
        [ForeignKey("ImageId")]
        [InverseProperty("GroupPostImageRelationship")]
        public virtual Image Image { get; set; }
        [ForeignKey("PostId")]
        [InverseProperty("GroupPostImageRelationships")]
        public virtual GroupPost Post { get; set; }
    }
}
