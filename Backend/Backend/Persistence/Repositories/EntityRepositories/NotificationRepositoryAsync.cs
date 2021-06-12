using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    public class NotificationRepositoryAsync : GenericRepositoryAsync<Notification>, INotificationRepositoryAsync
    {

        private readonly DbSet<Notification> _notifications;
        public NotificationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _notifications = dbContext.Set<Notification>();
        }


        public async Task<IReadOnlyList<Notification>> GetNotificationsOfUser(int userId, int pageNumber, int pageSize)
        {
            return await _notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

    }
}
