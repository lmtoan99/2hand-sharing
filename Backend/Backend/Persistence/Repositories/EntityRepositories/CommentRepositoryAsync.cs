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
    public class CommentRepositoryAsync : GenericRepositoryAsync<Comment>, ICommentRepositoryAsync
    {
        private readonly DbSet<Comment> _comments;
        public CommentRepositoryAsync(ApplicationDbContext context) : base(context)
        {
            _comments = context.Set<Comment>();
        }

        public async Task<IReadOnlyCollection<Comment>> GetListCommentByPostId(int postId, int pageNumber, int pageSize)
        {
            return await _comments.Where(c => c.PostId == postId)
                .OrderByDescending(c => c.PostTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.PostByAccount)
                .ThenInclude(a => a.Avatar)
                .ToListAsync();
        }
    }
}
