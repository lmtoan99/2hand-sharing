using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Group : BaseEntity
    {
        private string GroupName;
        private string Description;
        private DateTime CreateDate;
        private string Rules;
    }
}
