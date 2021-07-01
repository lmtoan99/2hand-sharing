using Application.DTOs.Group;
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
        Task<IReadOnlyList<Group>> GetAllGroupJoinedByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<GroupMemberDetail> GetMemberGroup(int groupId, int userId);
        Task<IReadOnlyList<GroupMemberDetail>> GetListJoinGroupRequestByGroupIdAsync(int pageNumber, int pageSize, int groupId);
        Task<IReadOnlyList<GroupMemberDetail>> GetInvitationListByUserIdAsync(int pageNumber, int pageSize, int userId);
    }
}
