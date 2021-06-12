using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Notification
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public string Data { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
