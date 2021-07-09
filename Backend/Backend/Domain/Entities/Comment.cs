using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public int PostByAccontId{get;set;}
        public DateTime PostTime{get;set;}
        public int PostId { get; set; }
        [ForeignKey("PostByAccontId")]
        [InverseProperty("Comments")]
        public virtual User PostByAccount { get; set; }
        [ForeignKey("PostId")]
        [InverseProperty("Comments")]
        public virtual GroupPost Post { get; set; }
    }
}
