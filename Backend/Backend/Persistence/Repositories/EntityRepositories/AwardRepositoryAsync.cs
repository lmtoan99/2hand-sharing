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
            return await _award.OrderByDescending(i => i.CreateTime).Take(10).ToListAsync();
        }


    }
}
