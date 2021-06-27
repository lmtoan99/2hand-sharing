using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class DonateEventInformationRepositoryAsync : GenericRepositoryAsync<DonateEventInformation>, IDonateEventInformationRepositoryAsync
    {
        private readonly DbSet<DonateEventInformation> _donateEventInformation;
        public DonateEventInformationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _donateEventInformation = dbContext.Set<DonateEventInformation>();
        }
    }
}
