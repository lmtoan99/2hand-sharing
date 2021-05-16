using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ReceiveItemInformationFeatures.Commands
{
    public class CancelReceiveItemCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class UpdateStatusCancelItemCommandHandler : IRequestHandler<CancelReceiveItemCommand, Response<string>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            private readonly IItemRepositoryAsync _itemRepository;
            public UpdateStatusCancelItemCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository, IItemRepositoryAsync itemRepository)
            {
                _receiveItemInformationRepository = receiveItemRepository;
                _itemRepository = itemRepository;
            }
            public async Task<Response<string>> Handle(CancelReceiveItemCommand command, CancellationToken cancellationToken)
            {
                var receiveItemInformation = await _receiveItemInformationRepository.GetReceiveRequestWithItemInfoById(command.Id);

                if (receiveItemInformation == null) throw new ApiException($"Receive Item Information Not Found."); 
                if (receiveItemInformation.ReceiverId != command.UserId) throw new UnauthorizedAccessException();
                if (receiveItemInformation.ReceiveStatus == (int)ReceiveItemInformationStatus.RECEIVING){
                    receiveItemInformation.Items.Status = (int)ItemStatus.NOT_YET;
                    await _itemRepository.UpdateAsync(receiveItemInformation.Items);
                    await _receiveItemInformationRepository.DeleteAsync(receiveItemInformation);
                    return new Response<string>($"Cancel success.");
                }
                if (receiveItemInformation.ReceiveStatus == (int)ReceiveItemInformationStatus.PENDING)
                {
                    await _receiveItemInformationRepository.DeleteAsync(receiveItemInformation);
                    return new Response<string>($"Cancel success.");
                }
                throw new ApiException($"You can not cancel receive item.");
            }
        }
    }
}