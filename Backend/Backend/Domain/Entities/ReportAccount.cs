using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ReportAccount : BaseEntity
    {
        public string Content { get; set; }
        public int ReportFromAccountId { get; set; }
        public int ReportToAccountId { get; set; }
        [ForeignKey("ReportFromAccountId")]
        [InverseProperty("ReportSends")]
        public virtual User ReportFromAccount { get; set; }
        [ForeignKey("ReportToAccountId")]
        [InverseProperty("ReportReceives")]
        public virtual User ReportToAccount { get; set; }
    }
}
