using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IReceiveItemInformationRepositoryAsync : IGenericRepositoryAsync<ReceiveItemInformation>
    {
        Task<IReadOnlyList<ReceiveItemInformation>> GetAllByItemId(int itemId);
        Task<ReceiveItemInformation> GetReceiveRequestWithItemInfoById(int requestId);

        Task<ReceiveItemInformation> GetReceiveRequestByItemIdAndUserId(int itemId, int userId);
    }
}
