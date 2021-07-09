using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Firebase
{
    public class AcceptInvitationData
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
