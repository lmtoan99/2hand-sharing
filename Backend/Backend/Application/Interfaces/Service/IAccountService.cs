using Application.DTOs.Account;
using Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IAccountService
    {
        Task<Response<RegisterResponse>> RegisterAsync(RegisterRequest request);
        public Task<Response<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest request);

    }
}
