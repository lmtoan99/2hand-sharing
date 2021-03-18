using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class GroupPostImageRelationship : BaseEntity
    {
        [ForeignKey("Image")]
        public int ImageId { get; set; }
        [ForeignKey("Post")]
        public int PostId{get;set;}
        public Image Image { get; set; }
        public GroupPost Post { get; set; }
    }
}
