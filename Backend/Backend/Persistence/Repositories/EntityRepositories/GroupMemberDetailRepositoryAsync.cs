using Application.DTOs.Group;
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
    public class GroupMemberDetailRepositoryAsync : GenericRepositoryAsync<GroupMemberDetail>, IGroupMemberDetailRepositoryAsync
    {
        private readonly DbSet<GroupMemberDetail> _groupMemberDetails;

        public GroupMemberDetailRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupMemberDetails = dbContext.Set<GroupMemberDetail>();
        }

        public async Task<IReadOnlyList<GroupMemberDetail>> GetAllGroupMemberByGroupIdAsync(int pageNumber, int pageSize, int groupId)
        {
            return await _groupMemberDetails
                .Where(i => ( i.GroupId == groupId))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Group>> GetAllGroupJoinedByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            return await _groupMemberDetails
                .Where(i=> (i.MemberId==userId))
                .Select(i=> new Group {Id=i.Group.Id, GroupName=i.Group.GroupName})
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<GroupMemberDetail> GetMemberGroup(int groupId, int userId)
        {
            return await _groupMemberDetails
                .Where(i => (i.MemberId == userId && i.GroupId == groupId)).FirstOrDefaultAsync();
        }
    }
}
