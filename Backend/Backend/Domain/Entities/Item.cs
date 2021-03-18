using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Item : BaseEntity
    {
        public string ItemName { get; set; }
        public string ReceiveAddress{get;set;}
        [ForeignKey("Category")]
        public int CategoryId{get;set;}
        [ForeignKey("DonateAccount")]
        public int DonateAccountId{get;set;}
        public DateTime PostTime { get; set; }
        public bool Status { get; set; }
        public Category Category { get; set; }
        public Account DonateAccount { get; set; }
    }
}
