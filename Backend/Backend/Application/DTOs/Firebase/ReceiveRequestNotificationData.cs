using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Firebase
{
    public class ReceiveRequestNotificationData
    {
        public int Id;
        public int ReceiverId;
        public string ReceiverName;
        public string ReceiverAvatarUrl;
        public int ItemId;
        public string ItemName;
        public string ReceiveReason;
        public DateTime CreateDate;
    }
}
