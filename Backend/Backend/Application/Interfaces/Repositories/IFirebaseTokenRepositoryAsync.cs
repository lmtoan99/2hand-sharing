using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFirebaseTokenRepositoryAsync : IGenericRepositoryAsync<FirebaseToken>
    {
        public Task<IReadOnlyList<string>> GetListFirebaseToken(int userId); 
    }
}
