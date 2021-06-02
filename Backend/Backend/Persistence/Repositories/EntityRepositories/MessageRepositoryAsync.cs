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
    public class MessageRepositoryAsync : GenericRepositoryAsync<Message>, IMessageRepositoryAsync
    {
        private readonly DbSet<Message> _message;
        public MessageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext) 
        {
            _message = dbContext.Set<Message>();
        }
        public async Task<IReadOnlyCollection<Message>> GetListMessage(int user1, int user2, int pageNumber, int pageSize)
        {
            return await _message
                .Where(m => (m.SendFromAccountId == user1 && m.SendToAccountId == user2) || (m.SendToAccountId == user1 && m.SendFromAccountId == user2))
                .OrderByDescending(m => m.SendDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Message>> GetRecentMessages(int userId, int pageNumber, int pageSize)
        {
            var group = _message
                .Where(m => m.SendToAccountId == userId)
                .GroupBy(m => m.SendFromAccountId)
                .Select(group => new { SendFromAccountId = group.Key, Max = group.Max(m => m.SendDate) });
            return await _message
                .Where(m => group.Where(group => group.SendFromAccountId == m.SendFromAccountId).First().Max == m.SendDate)
                .Include(m => m.SendFromAccount)
                .OrderByDescending(m => m.SendDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); ;
        }
    }
}
