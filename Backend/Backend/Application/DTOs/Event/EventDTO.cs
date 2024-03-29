﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Event
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Content { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupAvatar { get; set; }
    }
}
