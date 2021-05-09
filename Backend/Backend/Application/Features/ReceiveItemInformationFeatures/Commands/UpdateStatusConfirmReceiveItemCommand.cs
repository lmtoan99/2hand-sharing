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
    public class UpdateStatusConfirmReceiveItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class UpdateStatusConfirmSendItemCommandHandler : IRequestHandler<UpdateStatusConfirmReceiveItemCommand, Response<int>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            public UpdateStatusConfirmSendItemCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository, IMapper mapper)
            {
                _receiveItemInformationRepository = receiveItemRepository;
            }
            public async Task<Response<int>> Handle(UpdateStatusConfirmReceiveItemCommand command, CancellationToken cancellationToken)
            {
                var receiveItem = await _receiveItemInformationRepository.GetByIdAsync(command.Id);

                if (receiveItem == null) throw new ApiException($"Receive Item Information Not Found.");
 
                if (receiveItem.ReceiverId != command.UserId) throw new UnauthorizedAccessException();

                if (receiveItem.ReceiveStatus == (int)ReceiveItemInformationStatus.RECEIVING)
                {
                    receiveItem.ReceiveStatus = (int)ReceiveItemInformationStatus.SUCCESS;
                    await _receiveItemInformationRepository.UpdateAsync(receiveItem);
                    return new Response<int>(receiveItem.Id);
                }
                throw new ApiException($"You only can confirm received when you received item successfuly.");
            }
        }
    }
}
