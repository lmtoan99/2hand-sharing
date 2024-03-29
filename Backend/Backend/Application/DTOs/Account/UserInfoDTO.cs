﻿using Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public AddressDTO Address { get; set; }
        public string Email { get; set; }
    }
}
