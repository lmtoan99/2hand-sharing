﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Group
{
    public class AddMemberRequest
    {
        [Required]
        public int UserId { get; set; }

    }
}
