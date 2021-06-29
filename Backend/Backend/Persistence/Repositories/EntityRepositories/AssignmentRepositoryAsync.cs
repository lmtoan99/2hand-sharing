using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class AssignmentRepositoryAsync : GenericRepositoryAsync<Assignment>, IAssignmentRepositoryAsync
    {
        public AssignmentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
