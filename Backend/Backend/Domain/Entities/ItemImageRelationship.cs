using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ItemImageRelationship : BaseEntity
    {
        public int ImageId { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ImageId")]
        [InverseProperty("ItemImageRelationship")]
        public virtual Image Image { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("ItemImageRelationships")]
        public virtual Item Item { get; set; }
    }
}
