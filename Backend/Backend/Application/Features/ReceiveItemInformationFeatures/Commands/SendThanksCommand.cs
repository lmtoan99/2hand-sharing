using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ReceiveItemInformationFeatures.Commands
{
    public class SendThanksCommand : IRequest<Response<int>>
    {
        public int requestId { get; set; }
        public int userId { get; set; }
        public string thanks { get; set; }
    }
    public class SendThanksCommandHandler : IRequestHandler<SendThanksCommand, Response<int>>
    {
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemRepository;
        public SendThanksCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository)
        {
            _receiveItemRepository = receiveItemRepository;
        }
        public async Task<Response<int>> Handle(SendThanksCommand request, CancellationToken cancellationToken)
        {
            var receiveRequest = await _receiveItemRepository.GetByIdAsync(request.requestId);
            if (receiveRequest == null) throw new KeyNotFoundException("ReceiveId not found");
            if (receiveRequest.ReceiverId != request.userId) throw new UnauthorizedAccessException();
            //if (receiveRequest.ReceiveStatus != (int)ReceiveItemInformationStatus.SUCCESS) throw new ApiException("Receive request not success");

            receiveRequest.Thanks = request.thanks;
            await _receiveItemRepository.UpdateAsync(receiveRequest);
            return new Response<int>("Update successfully");
        }
    }
}
