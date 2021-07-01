using Application.DTOs.Account;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
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

        public async Task<IList<User>> GetListUserByQuery(string query, int pageNumber, int pageSize)
        {
            return await _user.FromSqlRaw($"select * from Users where match(FullName, PhoneNumber) against('${query}' in natural language mode)")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.Avatar)
                .ToListAsync();
        }

        public async Task<User> GetUserByAccountId(string accountId)
        {
            return await _user.Where(u => u.AccountId.Equals(accountId)).FirstOrDefaultAsync();
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

        public async Task<User> GetUserInfoByUserId(string id)
        {
            return await _user
                .Where(u => u.AccountId.Equals(id))
                .Include(u => u.Address)
                .Include(u => u.Avatar)
                .FirstOrDefaultAsync();
        }
    }
}
