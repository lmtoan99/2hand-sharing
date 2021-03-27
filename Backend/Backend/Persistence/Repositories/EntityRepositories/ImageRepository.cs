using Application.Interfaces.Repositories;
using Domain.Entities;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    class ImageRepository : GenericRepositoryAsync<Image>, IImageRepository
    {
        private readonly DbSet<Image> _image;
        private readonly string credentialFilePath = string.Format(@"{0}\twohandsharing-key.json", AppDomain.CurrentDomain.BaseDirectory);
        private readonly string bucketName = "twohandsharing.appspot.com";
        public ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _image = dbContext.Set<Image>();
        }

        public string GenerateV4UploadSignedUrl(string fileName)
        {
            UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credentialFilePath);

            var contentHeaders = new Dictionary<string, IEnumerable<string>>
            {
                { "Content-Type", new[] { "image/png"} }
            };

            // V4 is the default signing version.
            UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromMinutes(5));

            UrlSigner.RequestTemplate template = UrlSigner.RequestTemplate
                .FromBucket(bucketName)
                .WithObjectName(fileName)
                .WithHttpMethod(HttpMethod.Put)
                .WithContentHeaders(contentHeaders);

            string url = urlSigner.Sign(template, options);
            return url;
        }
        public string GenerateV4SignedReadUrl(string fileName)
        {
            string url = string.Format(@"https://storage.googleapis.com/{0}/{1}",bucketName, fileName);
            return url;
        }
    }
}
