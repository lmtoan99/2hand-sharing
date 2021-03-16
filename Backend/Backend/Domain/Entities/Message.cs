using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Message : BaseEntity
    {
        private string Content;
        private int SendDate;
        private int SendFrom;
        private int SendTo;
    }
}
