using Application.DTOs.Account;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAccountRepositoryAsync : IGenericRepositoryAsync<Account>
    {
    }
}
