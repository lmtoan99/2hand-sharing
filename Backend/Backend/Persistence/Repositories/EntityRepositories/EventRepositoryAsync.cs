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
    public class EventRepositoryAsync : GenericRepositoryAsync<Event>, IEventRepositoryAsync
    {
        private readonly DbSet<Event> _event;

        public EventRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _event = dbContext.Set<Event>();
        }

        public async Task<IReadOnlyList<Event>> GetAllGroupEventByGroupIdAsync(int pageNumber, int pageSize, int groupId)
        {
            return await _event
                .Where(e => e.GroupId == groupId)
                .OrderByDescending(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
