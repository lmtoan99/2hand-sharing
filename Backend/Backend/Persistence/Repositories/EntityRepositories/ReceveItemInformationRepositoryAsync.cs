using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    class ReceveItemInformationRepositoryAsync : GenericRepositoryAsync<ReceiveItemInformation>, IReceiveItemInformationRepositoryAsync
    {
        public ReceveItemInformationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
