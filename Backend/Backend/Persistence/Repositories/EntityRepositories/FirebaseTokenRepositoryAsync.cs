using Application.Interfaces.Repositories;
using Domain.Entities;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    public class FirebaseTokenRepositoryAsync : GenericRepositoryAsync<FirebaseToken>, IFirebaseTokenRepositoryAsync
    {
        private readonly DbSet<FirebaseToken> _firebaseTokens;
        public FirebaseTokenRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _firebaseTokens = dbContext.Set<FirebaseToken>();
        }

        public void CleanExpiredToken(IReadOnlyList<string> tokens, IReadOnlyList<SendResponse> responses)
        {
            for (int i = 0; i < responses.Count; i++)
            {
                var response = responses[i];
                if (!response.IsSuccess)
                {
                    _firebaseTokens.Remove(_firebaseTokens.Where(f => f.Token.Equals(tokens[i])).FirstOrDefault());
                }
            }
        }

        public async Task<IReadOnlyList<string>> GetListFirebaseToken(int userId)
        {
            return await _firebaseTokens.Where(f => f.UserId == userId)
                .Select(f => f.Token)
                .ToListAsync();
        }

    }
}
