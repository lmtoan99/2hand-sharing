using Application.DTOs.Firebase;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IFirebaseSerivce
    {
        Task<IReadOnlyList<SendResponse>> SendMessage(IReadOnlyList<string> registration_ids, MessageNotiData messageValue);
        Task<IReadOnlyList<SendResponse>> SendReceiveRequestNotification(IReadOnlyList<string> registration_ids, ReceiveRequestNotificationData receiveRequestData);
    }
}
