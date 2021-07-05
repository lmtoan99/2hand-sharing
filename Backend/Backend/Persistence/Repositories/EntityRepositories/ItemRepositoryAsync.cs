using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using System;

namespace Persistence.Repositories.EntityRepositories
{
    class ItemRepositoryAsync : GenericRepositoryAsync<Item>, IItemRepositoryAsync
    {
        private readonly DbSet<Item> _item;

        public ItemRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _item = dbContext.Set<Item>();
        }

        public async Task<IReadOnlyCollection<Item>> GetAllItemHaveRequestWithReceiverId(int receiverId, int pageNumber, int pageSize)
        {
            return await _item.Where(i => i.ReceiveItemInformations.Where(r => r.ReceiverId == receiverId).Count() > 0)
                .OrderByDescending(i => i.PostTime)
                .Skip((pageNumber - 1) * pageSize)

                .Take(pageSize)
                .Include(i => i.Address)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Item>> GetAllPostItemsAsync(int pageNumber, int pageSize)
        {
            return await _item
                .Where(i => i.DonateType == (int)EDonateType.DONATE_POST && i.Status != (int)ItemStatus.SUCCESS)
                .OrderByDescending(i => i.PostTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.Address)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Item>> GetAllPostItemsByCategoryIdAsync(int pageNumber, int pageSize, int categoryId)
        {
            return await _item
                .Where(i => (i.DonateType == (int)EDonateType.DONATE_POST) && i.CategoryId == categoryId && i.Status != (int)ItemStatus.SUCCESS)
                .OrderByDescending(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.Address)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();  
        }

        public async Task<IReadOnlyCollection<Item>> GetItemByDonateAccountId(int accountId, int pageNumber, int pageSize)
        {
            return await _item
                .Where(i => i.DonateAccountId == accountId)
                .OrderByDescending(i => i.PostTime)
                .Include(i => i.Address)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .Include(i => i.DonateEventInformation.Event)
                .ToListAsync();
        }

        public async Task<Item> GetItemDetailByIdAsync(int itemId)
        {
            return await _item.Where(i => i.Id == itemId)
                .Include(i => i.Address)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .FirstAsync();
        }

        public async Task<Item> GetItemWithReceiveRequestByIdAsync(int itemId)
        {
            return await _item.Where(i => i.Id == itemId).Include(i => i.ReceiveItemInformations).FirstAsync();
        }

        public async Task<Item> GetItemWithEvent(int itemId)
        {
            return await _item.Where(i => i.Id == itemId).Include(i => i.DonateEventInformation).FirstAsync();
        }

        public async Task<IReadOnlyList<Item>> GetAllItemDonateForEventAsync(int pageNumber, int pageSize, int eventId)
        {
            return await _item
                .Where(i => i.DonateType == (int)EDonateType.DONATE_EVENT && i.DonateEventInformation.EventId == eventId)
                .OrderBy( i => i.Status)
                .ThenByDescending(i => i.PostTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.Address)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Item>> GetAllMyDonationsInEventAsync(int pageNumber, int pageSize, int eventId, int userId)
        {
            return await _item
                .Where(i => i.DonateType == (int)EDonateType.DONATE_EVENT && i.DonateEventInformation.EventId == eventId && userId == i.DonateAccountId)
                .OrderBy(i => i.Status)
                .ThenByDescending(i => i.PostTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(i => i.Address)
                .Include(i => i.DonateAccount)
                .Include(i => i.ItemImageRelationships)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Item>> SearchPostItemsAsync(string query, int pageNumber, int pageSize)
        {
            return await _item
                .Where(i => i.DonateType == (int)EDonateType.DONATE_POST && i.Status != (int)ItemStatus.SUCCESS)
                .Where(i => EF.Functions.Match(i.ItemName,query, MySqlMatchSearchMode.NaturalLanguage))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
