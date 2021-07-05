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

        public async Task<IReadOnlyCollection<Event>> GetAllEventPagedResponse(int pageNumber, int pageSize)
        {
            return await _event
                .OrderByDescending(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Group)
                .ThenInclude(g => g.Avatar)
                .ToListAsync();
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

        public async Task<IReadOnlyCollection<Event>> SearchEventPagedResponse(string query, int pageNumber, int pageSize)
        {
            return await _event
                .Where(e => EF.Functions.Match(e.EventName, query, MySqlMatchSearchMode.NaturalLanguage))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Group)
                .ThenInclude(g => g.Avatar)
                .ToListAsync();
        }
    }
}
