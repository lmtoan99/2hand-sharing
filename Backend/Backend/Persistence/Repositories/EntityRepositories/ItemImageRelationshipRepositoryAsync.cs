using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    class ItemImageRelationshipRepositoryAsync : GenericRepositoryAsync<ItemImageRelationship>, IItemImageRelationshipRepositoryAsync
    {
        public ItemImageRelationshipRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
