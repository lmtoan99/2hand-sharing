using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ItemFeatures.Commands
{
    public partial class AcceptReceiveRequestCommand : IRequest<Response<string>>
    {
        public int itemId { get; set; }
        public int requestId { get; set; }
        public int userId { get; set; }
    }
    public class AcceptReceiveRequestCommandHandle : IRequestHandler<AcceptReceiveRequestCommand, Response<string>>
    {
        private readonly IItemRepositoryAsync _itemRepositoryAsync;
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepositoryAsync;
        public AcceptReceiveRequestCommandHandle(IItemRepositoryAsync itemRepositoryAsync, IReceiveItemInformationRepositoryAsync receiveItemInformationRepositoryAsync)
        {
            _itemRepositoryAsync = itemRepositoryAsync;
            _receiveItemInformationRepositoryAsync = receiveItemInformationRepositoryAsync;
        }
        public async Task<Response<string>> Handle(AcceptReceiveRequestCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepositoryAsync.GetByIdAsync(request.itemId);
            if (item == null) throw new KeyNotFoundException("ItemId not found");
            if (item.Status != (int)ItemStatus.NOT_YET) throw new ApiException("Item is not able to create receive request");
            if (item.DonateAccountId != request.userId) throw new UnauthorizedAccessException("Unauthorized to access this item");
            var receiveRequest = await _receiveItemInformationRepositoryAsync.GetByIdAsync(request.requestId);
            if (receiveRequest == null) throw new KeyNotFoundException("RequestId not found");

            item.Status = (int)ItemStatus.PENDING_FOR_RECEIVER;
            await _itemRepositoryAsync.UpdateAsync(item);
            receiveRequest.ReceiveStatus = (int)ReceiveItemInformationStatus.RECEIVING;
            await _receiveItemInformationRepositoryAsync.UpdateAsync(receiveRequest);
            return new Response<string>("");
        }
    }
}
