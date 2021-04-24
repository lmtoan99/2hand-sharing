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
    public class UpdateStatusCancelReceiveItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class UpdateStatusCancelItemCommandHandler : IRequestHandler<UpdateStatusCancelReceiveItemCommand, Response<int>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            private readonly IItemRepositoryAsync _itemRepositoryAsync;
            private readonly IMapper _mapper;
            public UpdateStatusCancelItemCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository, IItemRepositoryAsync itemRepository, IMapper mapper)
            {
                _receiveItemInformationRepository = receiveItemRepository;
                _itemRepositoryAsync = itemRepository;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(UpdateStatusCancelReceiveItemCommand command, CancellationToken cancellationToken)
            {
                var receiveItemInformation = await _receiveItemInformationRepository.GetReceiveRequestWithItemInfoById(command.Id);

                if (receiveItemInformation == null) throw new ApiException($"Receive Item Information Not Found.");
                
                if (receiveItemInformation.ReceiverId != command.UserId) throw new UnauthorizedAccessException();

                if (receiveItemInformation.ReceiveStatus == (int)ReceiveItemInformationStatus.RECEIVING){
                    receiveItemInformation.ReceiveStatus = (int)ReceiveItemInformationStatus.CANCEL;
                    receiveItemInformation.Items.Status = (int)ItemStatus.NOT_YET;
                    await _receiveItemInformationRepository.UpdateAsync(receiveItemInformation);
                    return new Response<int>(receiveItemInformation.Id);
                }
                else throw new ApiException($"You cannot cancel receive item");
            }
        }
    }
}
