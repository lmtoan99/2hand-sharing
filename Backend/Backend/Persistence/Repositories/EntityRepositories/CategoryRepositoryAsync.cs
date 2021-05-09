using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class CategoryRepositoryAsync : GenericRepositoryAsync<Category>, ICategoryRepositoryAsync
    {
        private readonly DbSet<Category> _category;
        public CategoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _category = dbContext.Set<Category>();
        }
    }
}
