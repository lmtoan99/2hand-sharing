using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class GroupMemberDetail : BaseEntity
    {
        private int AccountId;
        private int GroupId;
        private bool ReportStatus;
        private DateTime JoinDate;
    }
}
