﻿using Application.Interfaces.Repositories;
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
    public class GroupMemberDetailRepositoryAsync : GenericRepositoryAsync<GroupMemberDetail>, IGroupMemberDetailRepositoryAsync
    {
        private readonly DbSet<GroupMemberDetail> _item;

        public GroupMemberDetailRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _item = dbContext.Set<GroupMemberDetail>();
        }

        public async Task<IReadOnlyList<GroupMemberDetail>> GetAllGroupMemberByGroupIdAsync(int pageNumber, int pageSize, int groupId)
        {
            return await _item
                .Where(i => ( i.GroupId == groupId))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}