using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class GroupPostRepositoryAsync : GenericRepositoryAsync<GroupPost>, IGroupPostRepositoryAsync
    {
        private readonly DbSet<GroupPost> _groupPost;

        public GroupPostRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupPost = dbContext.Set<GroupPost>();
        }

    }
}
