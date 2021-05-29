using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Application.DTOs.Firebase
{
    public class RequestStatusNotificationData
    {
        public int ItemId;
        public int RequestId;
        public string ItemName;
        public ReceiveItemInformationStatus RequestStatus;
    }
}
