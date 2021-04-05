using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ItemReport : BaseEntity
    {
        public int ReportFromAccountId { get; set; }
        public int ReportToItemId { get; set; }
        public string Content { get; set; }
        [ForeignKey("ReportFromAccountId")]
        [InverseProperty("ItemReports")]
        public virtual User ReportFromAccount { get; set; }
        [ForeignKey("ReportToItemId")]
        [InverseProperty("ItemReports")]
        public virtual Item ReportToItem { get; set; }
    }
}
