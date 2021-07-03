using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    public class AssignmentRepositoryAsync : GenericRepositoryAsync<Assignment>, IAssignmentRepositoryAsync
    {
        private readonly DbSet<Assignment> _assignments;
        public AssignmentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _assignments = dbContext.Set<Assignment>();
        }

        public async Task<Assignment> GetAssignmentByItemId(int itemId)
        {
            return await _assignments.Where(a => a.DonateEventInformation.ItemId == itemId).FirstOrDefaultAsync();
        }
    }
}
