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
    public partial class CreateReceiveRequestCommand : IRequest<Response<int>>
    {
        public int ItemId { get; set; }
        public string ReceiveReason { get; set; }
        public int ReveiverId { get; set; }
    }
    public class CreateReceiveRequestCommandHandle : IRequestHandler<CreateReceiveRequestCommand, Response<int>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
        public CreateReceiveRequestCommandHandle(IItemRepositoryAsync itemRepository,
            IReceiveItemInformationRepositoryAsync receiveItemInformationRepository)
        {
            _itemRepository = itemRepository;
            _receiveItemInformationRepository = receiveItemInformationRepository;
        }
        public async Task<Response<int>> Handle(CreateReceiveRequestCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(request.ItemId);
            if (item == null) throw new KeyNotFoundException("ItemId not found");
            if (item.Status != (int)ItemStatus.NOT_YET) throw new ApiException("Item is not able to receive");

            var newInfo = new Domain.Entities.ReceiveItemInformation
            {
                ItemId = request.ItemId,
                ReceiverId = request.ReveiverId,
                ReceiveReason = request.ReceiveReason
            };
            await _receiveItemInformationRepository.AddAsync(newInfo);

            return new Response<int>(newInfo.Id);
        }
    }
}
