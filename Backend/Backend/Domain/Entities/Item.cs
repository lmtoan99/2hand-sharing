using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Item : BaseEntity
    {
        public string ItemName { get; set; }
        public string ReceiveAddress { get; set; }
        public int CategoryId { get; set; }
        public int DonateAccountId { get; set; }
        public DateTime PostTime { get; set; }
        public bool Status { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Items")]
        public virtual Category Category { get; set; }
        [ForeignKey("DonateAccountId")]
        [InverseProperty("DonateItems")]
        public virtual Account DonateAccount { get; set; }
        public virtual DonateEventInformation DonateEventInformation { get; set; }
        public virtual ICollection<ItemReport> ItemReports { get; set; }
        public virtual DonatePostInformation DonatePostInformation { get; set; }
        public virtual ICollection<ItemImageRelationship> ItemImageRelationships { get; set; }

    }
}
