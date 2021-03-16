    using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Event : BaseEntity
    {
        private string EventName;
        private DateTime StartDate;
        private DateTime EndDate;
        private string Content;
        private int GroupId;
    }
}
