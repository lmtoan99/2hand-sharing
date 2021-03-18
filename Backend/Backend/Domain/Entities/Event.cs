    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Event : BaseEntity
    {
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Content { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
