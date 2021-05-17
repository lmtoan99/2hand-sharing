using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Group
{
    public class CreateGroupRequest
    {
        [Required]
        public string GroupName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Rules { get; set; }
    }
}
