using Application.DTOs.Account;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    public class UserRepositoryAsync : GenericRepositoryAsync<User>, IUserRepositoryAsync
    {
        private readonly DbSet<User> _user;
        public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _user = dbContext.Set<User>();
        }

        public Task<User> GetUserByAccountId(string accountId)
        {
            return _user.Where(u => u.AccountId.Equals(accountId)).FirstOrDefaultAsync();
        }

        public async Task<string> GetUserFullnameById(int id)
        {
            return await _user.Where(u => u.Id == id)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserInfoById(int id)
        {
            return await _user.Where(u => u.Id == id)
                    .Include(u => u.Address)
                    .Include(u => u.Avatar)
                    .FirstOrDefaultAsync();
        }

        public Task<User> GetUserInfoByUserId(string id)
        {
            return _user
                .Where(u => u.AccountId.Equals(id))
                .Include(u => u.Address)
                .Include(u => u.Avatar)
                .FirstOrDefaultAsync();
        }
    }
}
