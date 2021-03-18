using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class ReportAccount : BaseEntity
    {
        public string Content { get; set; }
        [ForeignKey("ReportFromAccount")]
        public int ReportFromAccountId { get; set; }
        [ForeignKey("ReportToAccount")]
        public int ReportToAccountId { get; set; }
        public Account ReportFromAccount { get; set; }
        public Account ReportToAccount { get; set; }
    }
}
