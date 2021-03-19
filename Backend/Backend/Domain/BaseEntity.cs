using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}