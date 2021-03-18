using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class GroupPost : BaseEntity
    {
        public string Content { get; set; }
        public DateTime PostTime{get;set;}
        [ForeignKey("Group")]
        public int GroupId{get;set;}
        [ForeignKey("PostByAccount")]
        public int PostByAccountId{get;set;}
        public int Visibility{get;set;}
        public Group Group { get; set; }
        public Account PostByAccount { get; set; }
    }
}
