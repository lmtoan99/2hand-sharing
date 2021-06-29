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
    public class DonateEventInformationRepositoryAsync : GenericRepositoryAsync<DonateEventInformation>, IDonateEventInformationRepositoryAsync
    {
        private readonly DbSet<DonateEventInformation> _donateEventInformation;
        public DonateEventInformationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _donateEventInformation = dbContext.Set<DonateEventInformation>();
        }

        public async Task<DonateEventInformation> CheckPermissonForAssignItem(int donateEventId, int adminId, int memberId)
        {
            return await _donateEventInformation
                .Where(d => d.Id == donateEventId
                    && d.Event.Group.GroupAdminDetails.Any(d => d.AdminId == adminId)
                    && (d.Event.Group.GroupMemberDetails.Any(d => d.MemberId == memberId) || d.Event.Group.GroupAdminDetails.Any(d => d.AdminId == memberId)))
                .FirstOrDefaultAsync();
        }
    }
}
