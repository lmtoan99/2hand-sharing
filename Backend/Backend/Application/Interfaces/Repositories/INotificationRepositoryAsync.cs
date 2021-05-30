using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface INotificationRepositoryAsync : IGenericRepositoryAsync<Notification>
    {
        Task<IReadOnlyCollection<Notification>> GetNotificationsOfUser(int userId, int pageNumber, int pageSize);
    }
}
