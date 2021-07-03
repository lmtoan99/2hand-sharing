using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAssignmentRepositoryAsync : IGenericRepositoryAsync<Assignment>
    {
        public Task<IReadOnlyCollection<Assignment>> GetPagedAssignmentByEventIdAsync(int eventId, int pageNumber, int pageSize);
        public Task<Assignment> CheckAssignBefore(int userId);
    }
}
