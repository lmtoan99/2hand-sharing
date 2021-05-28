using Application.DTOs.Firebase;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
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
            private readonly IFirebaseSerivce _firebaseSerivce;
            private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
            public UpdateStatusCancelItemCommandHandler(IFirebaseSerivce firebaseSerivce, IFirebaseTokenRepositoryAsync firebaseTokenRepository, IReceiveItemInformationRepositoryAsync receiveItemRepository, IItemRepositoryAsync itemRepository)
            {
                _receiveItemInformationRepository = receiveItemRepository;
                _itemRepository = itemRepository;
                _firebaseSerivce = firebaseSerivce;
                _firebaseTokenRepository = firebaseTokenRepository;
            }
            public async Task<Response<string>> Handle(CancelReceiveItemCommand command, CancellationToken cancellationToken)
            {
                var receiveItemInformation = await _receiveItemInformationRepository.GetReceiveRequestWithItemInfoById(command.Id);

                if (receiveItemInformation == null) throw new ApiException($"Receive Item Information Not Found."); 
                if (receiveItemInformation.ReceiverId != command.UserId) throw new UnauthorizedAccessException();
                if (receiveItemInformation.ReceiveStatus == (int)ReceiveItemInformationStatus.RECEIVING){
                    receiveItemInformation.Items.Status = (int)ItemStatus.NOT_YET;
                    await _itemRepository.UpdateAsync(receiveItemInformation.Items);
                }
                await _receiveItemInformationRepository.DeleteAsync(receiveItemInformation);

                var tokens = await _firebaseTokenRepository.GetListFirebaseToken(receiveItemInformation.Items.DonateAccountId);
                if (tokens.Count > 0)
                {
                    CancelReceiveRequestNotificationData data = new CancelReceiveRequestNotificationData
                    {
                        RequestId = receiveItemInformation.Id,
                        ItemId = receiveItemInformation.ItemId
                    };
                    var responses = await _firebaseSerivce.SendCancelReceiveRequestNotification(tokens, data);
                    _firebaseTokenRepository.CleanExpiredToken(tokens, responses);

                }
                return new Response<string>($"Cancel success.");

            }
        }
    }
}