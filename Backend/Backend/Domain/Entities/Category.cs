using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
