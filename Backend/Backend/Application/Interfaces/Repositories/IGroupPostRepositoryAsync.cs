using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupPostRepositoryAsync : IGenericRepositoryAsync<GroupPost>
    {
        Task<IReadOnlyList<GroupPost>> GetAllPostInGroupAsync(int pageNumber, int pageSize, int groupId);
        Task<GroupPost> GetGroupPostForUpdatingById(int id);
    }
}
