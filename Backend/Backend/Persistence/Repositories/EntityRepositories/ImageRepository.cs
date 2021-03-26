using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    class ImageRepository : GenericRepositoryAsync<Image>, IImageRepository
    {
        private readonly DbSet<Image> _image;
        public ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _image = dbContext.Set<Image>();
        }
    }
}
