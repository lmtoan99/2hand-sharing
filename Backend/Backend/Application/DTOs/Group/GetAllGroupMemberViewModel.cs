﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Group
{
    public class GetAllGroupMemberViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
    }

}
