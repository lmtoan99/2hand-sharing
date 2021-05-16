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

namespace Application.Features.ReceiveItemInformationFeatures.Commands
{
    public class CancelChooseReceiverByIdCommand: IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class CancelChooseReceiverByIdCommandHandler : IRequestHandler<CancelChooseReceiverByIdCommand, Response<string>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            public CancelChooseReceiverByIdCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemInformationRepository)
            {
                _receiveItemInformationRepository = receiveItemInformationRepository;
            }
            public async Task<Response<string>> Handle(CancelChooseReceiverByIdCommand command, CancellationToken cancellationToken)
            {
                var receiveItemInformation = await _receiveItemInformationRepository.GetReceiveRequestWithItemInfoById(command.Id);

                if (receiveItemInformation == null) throw new ApiException($"Receive Item Information Not Found.");
                if (receiveItemInformation.Items.DonateAccountId != command.UserId) throw new UnauthorizedAccessException();

                if (receiveItemInformation.ReceiveStatus == (int)ReceiveItemInformationStatus.RECEIVING && receiveItemInformation.Items.Status == (int)ItemStatus.PENDING_FOR_RECEIVER)
                {
                    receiveItemInformation.ReceiveStatus = (int)ReceiveItemInformationStatus.PENDING;
                    receiveItemInformation.Items.Status = (int)ItemStatus.NOT_YET;
                    await _receiveItemInformationRepository.UpdateAsync(receiveItemInformation);
                    return new Response<string>($"Cancel choosing receiver successfully.");
                }
                throw new ApiException($"You can not cancel choosing receiver.");
            }
        }

    }
}
