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

        public async Task<IReadOnlyCollection<Assignment>> GetPagedAssignmentByEventIdAsync(int eventId, int pageNumber, int pageSize)
        {
            return await _assignments.Where(a => a.DonateEventInformation.EventId == eventId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.AssignByAccount)
                .Include(a => a.AssignedMember)
                .ToListAsync();
        }
        public async Task<Assignment> CheckAssignBefore(int userId)
        {
            return await _assignments.Where(a => a.AssignByAccountId == userId).FirstOrDefaultAsync();
        }
    }
}
