using Application.Interfaces.Repositories;
using Domain.Entities;
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

        public async Task<IReadOnlyList<string>> GetListFirebaseToken(int userId)
        {
            return await _firebaseTokens.Where(f => f.UserId == userId)
                .Select(f => f.Token)
                .ToListAsync();
        }
    }
}
