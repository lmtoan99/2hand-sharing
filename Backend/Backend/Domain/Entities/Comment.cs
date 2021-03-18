using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Comment : BaseEntity
    {
        public string Content { get; set; }
        [ForeignKey("PostByAccount")]
        public int PostBy{get;set;}
        public DateTime PostTime{get;set;}
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Account PostByAccount { get; set; }
        public GroupPost Post { get; set; }
    }
}
