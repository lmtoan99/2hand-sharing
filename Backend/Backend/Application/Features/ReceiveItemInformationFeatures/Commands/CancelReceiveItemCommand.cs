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
            private readonly INotificationRepositoryAsync _notificationRepository;
            public UpdateStatusCancelItemCommandHandler(IFirebaseSerivce firebaseSerivce, INotificationRepositoryAsync notificationRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, IReceiveItemInformationRepositoryAsync receiveItemRepository, IItemRepositoryAsync itemRepository)
            {
                _receiveItemInformationRepository = receiveItemRepository;
                _itemRepository = itemRepository;
                _firebaseSerivce = firebaseSerivce;
                _firebaseTokenRepository = firebaseTokenRepository;
                _notificationRepository = notificationRepository;
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
                        ItemId = receiveItemInformation.ItemId,
                    };
                    DefaultContractResolver contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };

                    var settings = new JsonSerializerSettings { ContractResolver = contractResolver, Formatting = Formatting.Indented };
                    var cancelReceiveItemData = JsonConvert.SerializeObject(data, settings);
                    var notifications = await _notificationRepository.GetByConditionAsync(n => n.UserId == receiveItemInformation.Items.DonateAccountId && receiveItemInformation.CreateDate.CompareTo(n.CreateTime) == 0);
                    if (notifications.Count > 0)
                    {
                        await _notificationRepository.DeleteAsync(notifications[0]);
                    }
                    var responses = await _firebaseSerivce.SendCancelReceiveRequestNotification(tokens, cancelReceiveItemData);
                    _firebaseTokenRepository.CleanExpiredToken(tokens, responses);

                }
                return new Response<string>($"Cancel success.");

            }
        }
    }
}