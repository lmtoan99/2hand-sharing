using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class ItemReport : BaseEntity
    {
        [ForeignKey("ReportFromAccount")]
        public int ReportFromAccountId { get; set; }
        [ForeignKey("ReportToItem")]
        public int ReportToItemId { get; set; }
        public string Content { get; set; }
        public Account ReportFromAccount { get; set; }
        public Item ReportToItem { get; set; }
    }
}
