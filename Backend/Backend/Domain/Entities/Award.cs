using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Award : BaseEntity
    {
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public DateTime CreateTime { get; set; }
        public int DonateTime { get; set; }
        public Account Account { get; set; }
    }
}
