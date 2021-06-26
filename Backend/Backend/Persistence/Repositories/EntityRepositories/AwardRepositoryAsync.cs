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
    class AwardRepositoryAsync : GenericRepositoryAsync<Award>, IAwardRepositoryAsync
    {
        private readonly DbSet<Award> _award;

        public AwardRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _award = dbContext.Set<Award>();
        }
        public async Task<IReadOnlyList<Award>> GetTopAwardAsync()
        {
            return await _award.OrderByDescending(a => a.CreateTime)
                .Select(a => a.AccountId)
                .Distinct()
                .Take(30)
                .Join(
                    _dbContext.Users,
                    a => a,
                    u => u.Id,
                    (a, u) => new
                    Award {
                        Id = 0,
                        CreateTime = DateTime.UtcNow,
                        AccountId = a,
                        Account = u
                    })
                .ToListAsync();
        }
    }
}
