using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories.EntityRepositories
{
    public class CommentRepositoryAsync : GenericRepositoryAsync<Comment>, ICommentRepositoryAsync
    {
        public CommentRepositoryAsync(ApplicationDbContext context) : base(context) { }
    }
}
