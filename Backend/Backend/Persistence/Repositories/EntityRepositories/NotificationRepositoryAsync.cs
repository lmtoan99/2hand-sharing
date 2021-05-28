using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories.EntityRepositories
{
    public class NotificationRepositoryAsync : GenericRepositoryAsync<Notification>, INotificationRepositoryAsync
    {
        public NotificationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
