using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Comment
{
    public class ListCommentDTO 
    {
        public string AvatarUrl { get; set; }
        public string PostByAccountName { get; set; }
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public int PostByAccontId { get; set; }
        public DateTime PostTime { get; set; }
    }
}
