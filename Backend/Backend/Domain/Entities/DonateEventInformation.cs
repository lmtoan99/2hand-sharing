using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class DonateEventInformation : BaseEntity
    {
        [ForeignKey("Event")]
        public int EventId { get; set; }
        [ForeignKey("Item")]
        public int ItemId{get;set;}
        public string Note{get;set;}
        public Item Item { get; set; }
        public Event Event { get; set; }
    }
}