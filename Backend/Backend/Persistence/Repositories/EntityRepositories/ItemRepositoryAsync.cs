using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using System;

namespace Persistence.Repositories.EntityRepositories
{
    class ItemRepositoryAsync : GenericRepositoryAsync<Item>, IItemRepositoryAsync
    {
        private readonly DbSet<Item> _item;
        public ItemRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _item = dbContext.Set<Item>();
        }

        public async Task<IReadOnlyList<Item>> GetAllPostItemsAsync(int pageNumber, int pageSize)
        {
            return await _item
                .Where(i => i.DonateType == (int)eDonateType.DonatePost)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
