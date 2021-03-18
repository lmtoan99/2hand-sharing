using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    class Image : BaseEntity
    {
        public string Url { get; set; }
    }
}
