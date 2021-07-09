using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IEventRepositoryAsync : IGenericRepositoryAsync<Event>
    {
        Task<IReadOnlyList<Event>> GetAllGroupEventByGroupIdAsync(int pageNumber, int pageSize, int groupId);
        Task<IReadOnlyCollection<Event>> GetAllEventPagedResponse(int pageNumber, int pageSize);
        Task<IReadOnlyCollection<Event>> SearchEventPagedResponse(string query, int pageNumber, int pageSize);

    }
}
