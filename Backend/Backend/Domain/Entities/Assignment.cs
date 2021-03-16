using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Assignment : BaseEntity
    {
        private DateTime AssignmentDate;
        private DateTime ExpirationDate;
        private int Status;
        private string Note;
        private int DonateEventInformationId;
        private int AssignedMember;
        private int AssignBy;
    }
}
