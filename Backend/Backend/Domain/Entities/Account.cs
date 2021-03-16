using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Account : BaseEntity
    {
        string Username;
        string Password;
        string FullName;
        DateTime Dob;
        string PhoneNumber;
        string Address;
        string Email;
    }
}
