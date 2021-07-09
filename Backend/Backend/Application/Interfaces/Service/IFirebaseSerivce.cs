using Application.DTOs.Firebase;
using Application.Enums;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IFirebaseSerivce
    {
        Task<IReadOnlyList<SendResponse>> SendMessage(IReadOnlyList<string> registration_ids, string messageValue, NotificationType type);

    }
}
