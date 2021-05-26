using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupMemberDetailRepositoryAsync: IGenericRepositoryAsync<GroupMemberDetail>
    {
        Task<IReadOnlyList<GroupMemberDetail>> GetAllGroupMemberByGroupIdAsync(int pageNumber, int pageSize, int groupId);
    }
}
