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
            private readonly IMapper _mapper;
            public UpdateStatusConfirmSendItemCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository, IMapper mapper)
            {
                _receiveItemInformationRepository = receiveItemRepository;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(UpdateStatusConfirmReceiveItemCommand command, CancellationToken cancellationToken)
            {
                var item = await _receiveItemInformationRepository.GetByIdAsync(command.Id);
                
                if (item == null)
                {
                    throw new ApiException($"Receive Item Information Not Found.");
                }
                if (item.ReceiverId != command.UserId)
                {
                    throw new UnauthorizedAccessException();
                }


                item.ReceiveStatus = (int)ReceiveItemInformationStatus.SUCCESS;
                await _receiveItemInformationRepository.UpdateAsync(item);
                return new Response<int>(item.Id);
            }
        }
    }
}
