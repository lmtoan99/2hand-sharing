using Application.DTOs.Firebase;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ItemFeatures.Commands
{
    public class UpdateStatusConfirmSendItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class UpdateStatusConfirmSendItemCommandHandler : IRequestHandler<UpdateStatusConfirmSendItemCommand, Response<int>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            private readonly IItemRepositoryAsync _itemRepository;
            private readonly IFirebaseSerivce _firebaseSerivce;
            private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
            private readonly INotificationRepositoryAsync _notificationRepository;
            private readonly IImageRepository _imageRepository;
            private readonly IAwardRepositoryAsync _awardRepository;
            public UpdateStatusConfirmSendItemCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemInformationRepository, 
                IItemRepositoryAsync itemRepository, 
                IFirebaseSerivce firebaseSerivce, 
                IFirebaseTokenRepositoryAsync firebaseTokenRepository, 
                INotificationRepositoryAsync notificationRepository,
                IImageRepository imageRepository,
                IAwardRepositoryAsync awardRepository)
            {
                _receiveItemInformationRepository = receiveItemInformationRepository;
                _itemRepository = itemRepository;
                _firebaseSerivce = firebaseSerivce;
                _firebaseTokenRepository = firebaseTokenRepository;
                _notificationRepository = notificationRepository;
                _imageRepository = imageRepository;
                _awardRepository = awardRepository;
            }

            public async Task<Response<int>> Handle(UpdateStatusConfirmSendItemCommand command, CancellationToken cancellationToken)
            {
                #region Validate
                var item = await _itemRepository.GetByIdAsync(command.Id);
                if (item == null) throw new ApiException($"Item Not Found.");
                if (item.DonateAccountId != command.UserId) throw new UnauthorizedAccessException();
                if (item.Status != (int)ItemStatus.PENDING_FOR_RECEIVER)
                {
                    throw new ApiException($"You can not confirm send.");
                }
                #endregion
                if (item.DonateType == (int)EDonateType.DONATE_POST)
                {
                    #region FindAcceptedRequestAndUserTokens
                    ReceiveItemInformation acceptedRequest = null;
                    var requests = await _receiveItemInformationRepository.GetAllByItemId(command.Id);
                    List<string> sendTokens = new List<string>();
                    for (int i = 0; i < requests.Count; i++)
                    {
                        if (requests[i].ReceiveStatus == (int)ReceiveItemInformationStatus.RECEIVING)
                        {
                            acceptedRequest = requests[i];
                        }
                        var tokens = await _firebaseTokenRepository.GetListFirebaseToken(requests[i].ReceiverId);
                        sendTokens.AddRange(tokens);
                    }
                    #endregion

                    #region SendNotification
                    ConfirmSentNotificationData data = new ConfirmSentNotificationData
                    {
                        ItemId = item.Id,
                        ItemName = item.ItemName,
                        ReceiverId = acceptedRequest.ReceiverId,
                        ReceiverName = acceptedRequest.Receiver.FullName,
                        ReceiverAvatarUrl = _imageRepository.GenerateV4SignedReadUrl(acceptedRequest.Receiver.Avatar?.FileName),
                    };
                    DefaultContractResolver contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver,
                        Formatting = Formatting.Indented
                    };
                    var confirmSentNotificationData = JsonConvert.SerializeObject(data, settings);
                    if (sendTokens.Count > 0)
                    {
                        var responses = await _firebaseSerivce.SendConfirmSentNotification(sendTokens, confirmSentNotificationData);
                        _firebaseTokenRepository.CleanExpiredToken(sendTokens, responses);
                    }
                    #endregion

                    #region SaveNotification
                    for (int i = 0; i < requests.Count; i++)
                    {
                        await _notificationRepository.AddAsync(new Notification
                        {
                            Type = "6",
                            Data = confirmSentNotificationData,
                            UserId = requests[i].ReceiverId,
                            CreateTime = DateTime.UtcNow
                        });
                    }
                    #endregion
                }
                #region CreateAward
                await _awardRepository.AddAsync(new Award { AccountId = item.DonateAccountId, CreateTime = DateTime.UtcNow });
                #endregion

                #region UpdateItem
                item.Status = (int)ItemStatus.SUCCESS;
                await _itemRepository.UpdateAsync(item);
                return new Response<int>(item.Id);
                #endregion
            }
        }
    }
}
