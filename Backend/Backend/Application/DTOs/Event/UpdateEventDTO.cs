using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Event
{
    public class UpdateEventDTO
    {
        public string EventName { get; set; }
        public DateTime EndDate { get; set; }
        public string Content { get; set; }
    }
}
