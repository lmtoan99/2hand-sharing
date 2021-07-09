using Application.DTOs.Firebase;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
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
    public class CancelChooseReceiverByIdCommand: IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class CancelChooseReceiverByIdCommandHandler : IRequestHandler<CancelChooseReceiverByIdCommand, Response<string>>
        {
            private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
            private readonly IFirebaseSerivce _firebaseSerivce;
            private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
            private readonly INotificationRepositoryAsync _notificationRepository;

            public CancelChooseReceiverByIdCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemInformationRepository, IFirebaseSerivce firebaseSerivce, IFirebaseTokenRepositoryAsync firebaseTokenRepository, INotificationRepositoryAsync notificationRepository)
            {
                _receiveItemInformationRepository = receiveItemInformationRepository;
                _firebaseSerivce = firebaseSerivce;
                _firebaseTokenRepository = firebaseTokenRepository;
                _notificationRepository = notificationRepository;
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
                    DefaultContractResolver contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver,
                        Formatting = Formatting.Indented
                    };

                    RequestStatusNotificationData data = new RequestStatusNotificationData
                    {
                        ItemId = receiveItemInformation.ItemId,
                        ItemName = receiveItemInformation.Items.ItemName,
                        RequestId = receiveItemInformation.Id,
                        RequestStatus = ReceiveItemInformationStatus.PENDING,
                    };
                    var messageData = JsonConvert.SerializeObject(data, settings);

                    await _notificationRepository.AddAsync(new Notification
                    {
                        Type = "4",
                        Data = messageData,
                        UserId = receiveItemInformation.ReceiverId,
                        CreateTime = DateTime.UtcNow
                    });
                    var tokens = await _firebaseTokenRepository.GetListFirebaseToken(receiveItemInformation.ReceiverId);
                    if (tokens.Count > 0)
                    {

                        var responses = await _firebaseSerivce.SendMessage(tokens, messageData, NotificationType.REQUEST_STATUS);
                        _firebaseTokenRepository.CleanExpiredToken(tokens, responses);

                    }
                    return new Response<string>($"Cancel choosing receiver successfully.");
                }
                throw new ApiException($"You can not cancel choosing receiver.");
            }
        }

    }
}
