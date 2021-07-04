using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.GroupPost
{
    public class GroupPostRequest
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int Visibility { get; set; }

        [Required]
        public int ImageNumber { get; set; }
    }
}
