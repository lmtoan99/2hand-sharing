using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Assignment
{
    public class AssignmentViewDTO
    {
        public int Id { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
        public int DonateEventInformationId { get; set; }
        public int AssignedMemberId { get; set; }
        public string AssignedMemberName { get; set; }
        public int AssignByAccountId { get; set; }
        public string AssignByAccountName { get; set; }
    }
}
