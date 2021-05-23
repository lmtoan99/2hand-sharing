using Application.DTOs.Firebase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IFirebaseSerivce
    {
        Task<int> SendMessage(IReadOnlyList<string> registration_id, MessageNotiData messageValue);
    }
}
