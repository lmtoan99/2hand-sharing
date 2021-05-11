using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class GroupAdminDetailRepositoryAsync : GenericRepositoryAsync<GroupAdminDetail>, IGroupAdminDetailRepositoryAsync
    {
        public GroupAdminDetailRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
