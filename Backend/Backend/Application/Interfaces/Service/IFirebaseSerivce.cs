using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Service
{
    public interface IFirebaseSerivce
    {
        void SendMessage(IReadOnlyList<string> registration_id, string messageValue);
    }
}
