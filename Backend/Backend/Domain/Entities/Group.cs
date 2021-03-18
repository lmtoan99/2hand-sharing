using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class Group : BaseEntity
    {
        public string GroupName{get;set;}
        public string Description{get;set;}
        public DateTime CreateDate{get;set;}
        public string Rules { get; set; }
    }
}
