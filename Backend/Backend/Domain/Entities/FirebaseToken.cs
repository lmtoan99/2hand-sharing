using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class FirebaseToken : BaseEntity
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("FirebaseToken")]
        public virtual User User { get; set; }
    }
}
