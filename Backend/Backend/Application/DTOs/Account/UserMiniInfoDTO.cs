using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class UserMiniInfoDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
