using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class DonatePostInformation : BaseEntity
    {
        public string Description { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("DonatePostInformation")]
        public virtual Item Item { get; set; }
        public virtual ICollection<ReceiveItemInformation> ReceiveItemInformations { get; set; }
    }
}
