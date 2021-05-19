using Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO Address { get; set; }
    }
}
