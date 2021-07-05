using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.GroupPost
{
    public class GetAllGroupPostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public List<string> ImageUrl { get; set; }
        public int PostByAccountId { get; set; }
        public string PostByAccountName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
