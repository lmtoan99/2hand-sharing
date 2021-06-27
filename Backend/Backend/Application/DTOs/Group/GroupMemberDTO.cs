using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Group
{
    public class GroupMemberDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int GroupId { get; set; }
        public int ReportStatus { get; set; }
        public DateTime JoinDate { get; set; }
        public string AvatarUrl { get; set; }
    }
}
