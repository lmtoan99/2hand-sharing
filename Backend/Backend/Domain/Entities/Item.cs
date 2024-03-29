﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Item : BaseEntity
    {
        public string ItemName { get; set; }
        public int AddressId { get; set; }
        public int CategoryId { get; set; }
        public int DonateAccountId { get; set; }
        public DateTime PostTime { get; set; }
        public int Status { get; set; }
        public int DonateType { get; set; }
        public string Description { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Items")]
        public virtual Category Category { get; set; }
        [ForeignKey("DonateAccountId")]
        [InverseProperty("DonateItems")]
        public virtual User DonateAccount { get; set; }
        [ForeignKey("AddressId")]
        [InverseProperty("DonateAddress")]
        public virtual Address Address { get; set; }
        public virtual DonateEventInformation DonateEventInformation { get; set; }
        public virtual ICollection<ItemReport> ItemReports { get; set; }
        public virtual ICollection<ReceiveItemInformation> ReceiveItemInformations { get; set; }
        public virtual ICollection<ItemImageRelationship> ItemImageRelationships { get; set; }

    }
}
