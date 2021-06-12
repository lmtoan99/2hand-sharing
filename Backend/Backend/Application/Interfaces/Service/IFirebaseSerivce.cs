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
        Task<IReadOnlyList<SendResponse>> SendMessage(IReadOnlyList<string> registration_ids, string messageValue);
        Task<IReadOnlyList<SendResponse>> SendThanksMessage(IReadOnlyList<string> registration_ids, string messageValue);
        Task<IReadOnlyList<SendResponse>> SendReceiveRequestNotification(IReadOnlyList<string> registration_ids, string receiveRequestData);
        Task<IReadOnlyList<SendResponse>> SendCancelReceiveRequestNotification(IReadOnlyList<string> registration_ids, string cancelReceiveRequestData);
        Task<IReadOnlyList<SendResponse>> SendReceiveRequestStatusNotification(IReadOnlyList<string> registration_ids, string cancelReceiveRequestData);
        Task<IReadOnlyList<SendResponse>> SendConfirmSentNotification(IReadOnlyList<string> registration_ids, string confirmSentData);


    }
}
