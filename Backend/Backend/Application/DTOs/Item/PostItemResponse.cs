using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Item
{
    public class PostItemResponse
    {
        public int Id { get; set; }
        public List<ImageUpload> ImageUploads { get; set; }
        public class ImageUpload
        {
            public string ImageName { get; set; }
            public string PresignUrl { get; set; }
        }
    }
}
