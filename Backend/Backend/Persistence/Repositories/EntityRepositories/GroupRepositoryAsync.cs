using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    public class GroupRepositoryAsync : GenericRepositoryAsync<Group>, IGroupRepositoryAsync
    {
        private readonly DbSet<Group> _group;

        public GroupRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _group = dbContext.Set<Group>();
        }

        public async Task<IReadOnlyList<Group>> GetAllGroupAsync(int pageNumber, int pageSize)
        {
            return await _group
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
