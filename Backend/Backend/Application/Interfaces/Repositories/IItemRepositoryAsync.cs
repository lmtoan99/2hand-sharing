using Domain.Entities;
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
        Task<IReadOnlyCollection<Item>> GetItemByDonateAccountId(int accountId, int pageNumber, int pageSize);
        Task<IReadOnlyCollection<Item>> GetAllItemHaveRequestWithReceiverId(int receiverId, int pageNumber, int pageSize);
        Task<IReadOnlyList<Item>> GetAllMyDonationsInEventAsync(int pageNumber, int pageSize, int eventId, int userId);
        Task<Item> GetItemWithEvent(int itemId);
        Task<Item> GetItemDetailByIdAsync(int itemId);
        Task<IReadOnlyList<Item>> GetAllItemDonateForEventAsync(int pageNumber, int pageSize, int eventId);
        Task<IReadOnlyCollection<Item>> SearchPostItemsAsync(string query, int pageNumber, int pageSize);
    }
}
