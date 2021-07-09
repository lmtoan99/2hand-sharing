using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupAdminDetailRepositoryAsync : IGenericRepositoryAsync<GroupAdminDetail>
    {
        Task<GroupAdminDetail> GetInfoGroupAdminDetail(int groupId, int adminId);
        Task<List<GroupAdminDetail>> GetListAdminByGroupId(int groupId, int pageNumber, int pageSize);
        Task<List<GroupAdminDetail>> GetAdminsByGroupId(int groupId);
    }
}
