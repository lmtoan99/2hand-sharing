using Application.DTOs.Account;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.EntityRepositories
{
    public class AccountRepositoryAsync : GenericRepositoryAsync<Account>, IAccountRepositoryAsync
    {
        private readonly DbSet<Account> _account;
        public AccountRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _account = dbContext.Set<Account>();
        }
    }
}
