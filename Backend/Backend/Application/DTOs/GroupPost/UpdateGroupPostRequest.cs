using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.GroupPost
{
    public class UpdateGroupPostRequest
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int Visibility { get; set; }
        public int ImageNumber { get; set; }
        public List<string> DeletedImages { get; set; }
    }
}
