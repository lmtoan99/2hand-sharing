using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DonateEventInformation> DonateEventInformation { get; set; }
        public DbSet<DonatePostInformation> DonatePostInformation { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAdminDetail> GroupAdminDetails { get; set; }
        public DbSet<GroupMemberDetail> GroupMemberDetails { get; set; }
        public DbSet<GroupPost> GroupPosts { get; set; }
        public DbSet<GroupPostImageRelationship> GroupPostImageRelationships { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImageRelationship> ItemImageRelationships { get; set; }
        public DbSet<ItemReport> ItemReports { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ReceiveItemInformation> ReceiveItemInformation { get; set; }
        public DbSet<ReportAccount> ReportAccounts { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
