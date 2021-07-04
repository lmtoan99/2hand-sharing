using Application.Enums;
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
    public class GroupPostRepositoryAsync : GenericRepositoryAsync<GroupPost>, IGroupPostRepositoryAsync
    {
        private readonly DbSet<GroupPost> _groupPost;

        public GroupPostRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupPost = dbContext.Set<GroupPost>();
        }

        public async Task<IReadOnlyList<GroupPost>> GetAllPublicPostInGroupAsync(int pageNumber, int pageSize, int groupId)
        {
            return await _groupPost
                .Where(e => e.GroupId == groupId && e.Visibility == (int)GroupPostType.PUBLIC_POST)
                .OrderByDescending(i => i.PostTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.PostByAccount)
                .Include(i => i.GroupPostImageRelationships)
                .ToListAsync();
        }
    }
}
