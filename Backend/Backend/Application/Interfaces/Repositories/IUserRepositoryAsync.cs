using Application.DTOs.Account;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
    {
        public Task<User> GetUserByAccountId(string accountId);
        Task<User> GetUserInfoByUserId(string id);
        Task<User> GetUserInfoById(int id);
    }
}
