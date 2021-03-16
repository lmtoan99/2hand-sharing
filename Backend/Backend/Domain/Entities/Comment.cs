using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Comment : BaseEntity
    {
        private string Content;
        private int PostBy;
        private DateTime PostTime;
    }
}
