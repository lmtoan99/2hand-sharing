using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class AuthenticateResponse
    {
        public string JWToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
