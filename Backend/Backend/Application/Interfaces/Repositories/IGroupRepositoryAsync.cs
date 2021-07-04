using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupRepositoryAsync : IGenericRepositoryAsync<Group>
    {
        Task<IReadOnlyList<Group>> GetAllGroupAsync(int pageNumber, int pageSize);
        Task<List<Group>> GetAllJoinedGroupByUserId(int userId,int pageNumber,int pageSize);
        Task<bool> CheckUserInGroup(int groupId, int userId);
    }
}
