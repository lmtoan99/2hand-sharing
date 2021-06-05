using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class UpdateAvatarResponse
    {
        public int Id { get; set; }
        public ImageUpload ImageUploads { get; set; }
        public class ImageUpload
        {
            public string ImageName { get; set; }
            public string PresignUrl { get; set; }
        }
    }
}
