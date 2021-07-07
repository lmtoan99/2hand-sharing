using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Group
{
    public class UpdateGroupDTO
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
    }
}
