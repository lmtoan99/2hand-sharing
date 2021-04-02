using Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IEmailService
    {
        //Task SendAsync(EmailRequest request);
        Task SendAsync(EmailRequest mail);
        //Task SendAsync(string email, string subject, string htmlMessage);
    }
}
