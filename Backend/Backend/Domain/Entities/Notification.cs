using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
