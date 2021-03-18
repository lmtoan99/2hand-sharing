using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class ItemImageRelationship : BaseEntity
    {
        [ForeignKey("Image")]
        public int ImageId { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Image Image { get; set; }
        public Item Item { get; set; }
    }
}
