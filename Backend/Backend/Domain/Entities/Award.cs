using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Award : BaseEntity
    {
        public int? AccountId { get; set; }
        public DateTime CreateTime { get; set; }
        [ForeignKey("AccountId")]
        [InverseProperty("Awards")]
        public virtual User Account { get; set; }
    }
}
