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
        Task<Response<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest request);
        Task<Response<string>> RegisterAsync(RegisterRequest request);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
    }
}
