﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IItemRepositoryAsync : IGenericRepositoryAsync<Item>
    {
        Task<IReadOnlyList<Item>> GetAllPostItemsAsync(int pageNumber, int pageSize);
        Task<IReadOnlyList<Item>> GetAllPostItemsByCategoryIdAsync(int pageNumber, int pageSize, int categoryId);
        Task<Item> GetItemWithReceiveRequestByIdAsync(int itemId);
        Task<IReadOnlyCollection<Item>> GetItemByDonateAccountId(int accountId);
        Task<IReadOnlyCollection<Item>> GetAllItemHaveRequestWithReceiverId(int receiverId, int pageNumber, int pageSize);
        Task<Item> GetItemContactByIdAsync(int itemId);
    }
}
