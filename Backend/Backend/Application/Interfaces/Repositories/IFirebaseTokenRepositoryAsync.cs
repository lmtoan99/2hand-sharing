using Domain.Entities;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFirebaseTokenRepositoryAsync : IGenericRepositoryAsync<FirebaseToken>
    {
        public Task<IReadOnlyList<string>> GetListFirebaseToken(int userId);
        public void CleanExpiredToken(IReadOnlyList<string> tokens, IReadOnlyList<SendResponse> responses);
    }
}
