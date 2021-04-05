using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class RegisterResponse
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
