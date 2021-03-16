using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class GroupPost : BaseEntity
    {
        private string Content;
        private DateTime PostTime;
        private int GroupId;
        private int PostBy;
        private int Visibility;
    }
}
