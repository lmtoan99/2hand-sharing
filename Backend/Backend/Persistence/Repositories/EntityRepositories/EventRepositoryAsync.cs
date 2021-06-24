using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class EventRepositoryAsync : GenericRepositoryAsync<Event>, IEventRepositoryAsync
    {
        public EventRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
