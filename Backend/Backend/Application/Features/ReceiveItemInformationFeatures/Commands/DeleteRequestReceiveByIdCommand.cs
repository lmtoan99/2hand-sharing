using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ReceiveItemInformationFeatures.Commands
{
    public class DeleteRequestReceiveByIdCommand: IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class DeleteRequestReceiveByIdCommandHandler : IRequestHandler<DeleteRequestReceiveByIdCommand, Response<int>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            public DeleteRequestReceiveByIdCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemInformationRepository)
            {
                _receiveItemInformationRepository = receiveItemInformationRepository;
            }
            public async Task<Response<int>> Handle(DeleteRequestReceiveByIdCommand command, CancellationToken cancellationToken)
            {
                var receiveItemInformation = await _receiveItemInformationRepository.GetByIdAsync(command.Id);
                if (receiveItemInformation == null) throw new ApiException($"Receive Item Information Not Found.");
                if (receiveItemInformation.ReceiverId != command.UserId) throw new UnauthorizedAccessException();

                await _receiveItemInformationRepository.DeleteAsync(receiveItemInformation);
                return new Response<int>(receiveItemInformation.Id);
            }
        }

    }
}
