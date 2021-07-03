using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Assignment
{
    public class AssignMemberDTO
    {
        public DateTime ExpirationDate { get; set; }
        public string Note { get; set; }
        public int DonateEventInformationId { get; set; }
        public int AssignedMemberId { get; set; }
    }
}
