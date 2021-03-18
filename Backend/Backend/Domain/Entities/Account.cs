using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Account : BaseEntity
    {
        public string Username{get;set;}
        public string Password{get;set;}
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber{get;set;}
        public string Address{get;set;}
        public string Email{get;set;}
    }
}
