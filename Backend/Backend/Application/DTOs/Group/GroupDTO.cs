using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Group
{
    public class GroupDTO
    {
        public int id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Rules { get; set; }

        public string AvatarUrl { get; set; }
    }
}
