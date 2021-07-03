using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Group
{
    public class Invitation
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime InvitationTime { get; set; }
    }
}
