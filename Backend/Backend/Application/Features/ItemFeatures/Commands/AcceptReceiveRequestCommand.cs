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

namespace Application.Features.ItemFeatures.Commands
{
    public partial class AcceptReceiveRequestCommand : IRequest<Response<int>>
    {
        public int requestId { get; set; }
        public int userId { get; set; }
    }
    public class AcceptReceiveRequestCommandHandle : IRequestHandler<AcceptReceiveRequestCommand, Response<int>>
    {
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepositoryAsync; 
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly INotificationRepositoryAsync _notificationRepository;

        public AcceptReceiveRequestCommandHandle(IReceiveItemInformationRepositoryAsync receiveItemInformationRepositoryAsync, IFirebaseSerivce firebaseSerivce, IFirebaseTokenRepositoryAsync firebaseTokenRepository, INotificationRepositoryAsync notificationRepository)
        {
            _receiveItemInformationRepositoryAsync = receiveItemInformationRepositoryAsync;
            _firebaseSerivce = firebaseSerivce;
            _firebaseTokenRepository = firebaseTokenRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<Response<int>> Handle(AcceptReceiveRequestCommand request, CancellationToken cancellationToken)
        {
            var receiveRequest = await _receiveItemInformationRepositoryAsync.GetReceiveRequestWithItemInfoById(request.requestId);

            if (receiveRequest == null) throw new KeyNotFoundException("RequestId not found");
            if (receiveRequest.Items.Status != (int)ItemStatus.NOT_YET) throw new ApiException("Item is not able to create receive request");
            if (receiveRequest.Items.DonateAccountId != request.userId) throw new UnauthorizedAccessException("Unauthorized to access this item");

            receiveRequest.Items.Status = (int)ItemStatus.PENDING_FOR_RECEIVER;
            receiveRequest.ReceiveStatus = (int)ReceiveItemInformationStatus.RECEIVING;
            await _receiveItemInformationRepositoryAsync.UpdateAsync(receiveRequest);
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
                ItemId = receiveRequest.ItemId,
                ItemName = receiveRequest.Items.ItemName,
                RequestId = receiveRequest.Id,
                RequestStatus = ReceiveItemInformationStatus.RECEIVING,
            };
            var messageData = JsonConvert.SerializeObject(data, settings);

            await _notificationRepository.AddAsync(new Notification
            {
                Type = "4",
                Data = messageData,
                UserId = receiveRequest.ReceiverId,
                CreateTime = DateTime.UtcNow
            });
            var tokens = await _firebaseTokenRepository.GetListFirebaseToken(receiveRequest.ReceiverId);
            if (tokens.Count > 0)
            {

                var responses = await _firebaseSerivce.SendMessage(tokens, messageData, NotificationType.REQUEST_STATUS);
                _firebaseTokenRepository.CleanExpiredToken(tokens, responses);

            }
            return new Response<int>(receiveRequest.Id);
        }
    }
}
