using Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest mail);
    }
}
