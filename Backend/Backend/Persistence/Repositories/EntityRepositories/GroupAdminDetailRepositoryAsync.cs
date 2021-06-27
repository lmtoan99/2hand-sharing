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
    public class GroupAdminDetailRepositoryAsync : GenericRepositoryAsync<GroupAdminDetail>, IGroupAdminDetailRepositoryAsync
    {
        private readonly DbSet<GroupAdminDetail> _groupAdminDetails;
        public GroupAdminDetailRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupAdminDetails = dbContext.Set<GroupAdminDetail>();
        }
        public async Task<GroupAdminDetail> GetInfoGroupAdminDetail(int groupId, int adminId)
        {
            return await _groupAdminDetails.Where(i => i.AdminId == adminId && i.GroupId == groupId).FirstOrDefaultAsync();
        }

        public async Task<List<GroupAdminDetail>> GetListAdminByGroupId(int groupId, int pageNumber, int pageSize)
        {
            return await _groupAdminDetails
                .Where(i => i.GroupId == groupId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.Admin.Avatar)
                .ToListAsync();
        }
    }
}
