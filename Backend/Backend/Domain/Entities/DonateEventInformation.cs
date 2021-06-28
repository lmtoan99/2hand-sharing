using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class DonateEventInformation : BaseEntity
    {
        public int EventId { get; set; }
        public int ItemId{get;set;}
        [ForeignKey("ItemId")]
        [InverseProperty("DonateEventInformation")]
        public virtual Item Item { get; set; }
        [ForeignKey("EventId")]
        [InverseProperty("DonateEventInformations")]
        public virtual Event Event { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}