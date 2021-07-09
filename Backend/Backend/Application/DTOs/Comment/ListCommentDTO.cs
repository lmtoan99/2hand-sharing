using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Comment
{
    public class ListCommentDTO : CommentDTO
    {
        public string AvatarUrl { get; set; }
        public string PostByAccountName { get; set; }
    }
}
