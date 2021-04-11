using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Image : BaseEntity
    {
        public string FileName { get; set; }
        public virtual GroupPostImageRelationship GroupPostImageRelationship { get; set; }
        public virtual ItemImageRelationship ItemImageRelationship { get; set; }
        public virtual User User { get; set; }
    }
}
