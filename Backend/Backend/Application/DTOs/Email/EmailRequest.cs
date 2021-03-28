using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Email
{
    //Mail Content
    public class EmailRequest
    {
        public string To { get; set; }// địa chỉ gửi đến
        public string Subject { get; set; }// tiêu đề
        public string Body { get; set; }// nội dung(hỗ trợ HTML) của gmail
        //public string From { get; set; }
    }
}
