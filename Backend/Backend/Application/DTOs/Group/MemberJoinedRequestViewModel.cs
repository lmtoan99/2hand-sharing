using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Group
{
    public class MemberJoinedRequestViewModel
    {
        public int RequesterId { get; set; }
        public string RequesterName { get; set; }
        public MemberJoinStatus JoinStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string AvatarUrl { get; set; }
    }
}
