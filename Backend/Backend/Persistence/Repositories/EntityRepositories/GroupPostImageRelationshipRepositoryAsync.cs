using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class GroupPostImageRelationshipRepositoryAsync : GenericRepositoryAsync<GroupPostImageRelationship>, IGroupPostImageRelationshipRepositoryAsync
    {
        private readonly DbSet<GroupPostImageRelationship> _groupPostImageRelationship;

        public GroupPostImageRelationshipRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupPostImageRelationship = dbContext.Set<GroupPostImageRelationship>();
        }

    }
}
