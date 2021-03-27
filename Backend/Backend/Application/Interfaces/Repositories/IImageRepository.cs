using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IImageRepository : IGenericRepositoryAsync<Image>
    {
        public string GenerateV4UploadSignedUrl(string fileName);
        public string GenerateV4SignedReadUrl(string fileName);
    }
}
