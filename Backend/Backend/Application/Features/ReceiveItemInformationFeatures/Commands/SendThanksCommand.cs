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
        private readonly INotificationRepositoryAsync _notificationRepository;
        private readonly IMessageRepositoryAsync _messageRepository;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IImageRepository _imageRepository;

        public SendThanksCommandHandler(INotificationRepositoryAsync notificationRepository, 
            IReceiveItemInformationRepositoryAsync receiveItemRepository, 
            IMessageRepositoryAsync messageRepository, 
            IFirebaseTokenRepositoryAsync firebaseTokenRepository, 
            IUserRepositoryAsync userRepository, 
            IFirebaseSerivce firebaseSerivce,
            IImageRepository imageRepository)
        {
            _receiveItemRepository = receiveItemRepository;
            _messageRepository = messageRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _userRepository = userRepository;
            _firebaseSerivce = firebaseSerivce;
            _notificationRepository = notificationRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Response<int>> Handle(SendThanksCommand request, CancellationToken cancellationToken)
        {
            var receiveRequest = await _receiveItemRepository.GetReceiveRequestWithItemInfoById(request.requestId);
            if (receiveRequest == null) throw new KeyNotFoundException("ReceiveId not found");
            if (receiveRequest.ReceiverId != request.userId) throw new UnauthorizedAccessException();
            //if (receiveRequest.ReceiveStatus != (int)ReceiveItemInformationStatus.SUCCESS) throw new ApiException("Receive request not success");
            var result = await _messageRepository.AddAsync(new Domain.Entities.Message
            {
                Content = request.thanks,
                SendFromAccountId = receiveRequest.ReceiverId,
                SendToAccountId = receiveRequest.Items.DonateAccountId,
                SendDate = DateTime.Now.ToUniversalTime()
            });
            var tokens = await _firebaseTokenRepository.GetListFirebaseToken(receiveRequest.Items.DonateAccountId);
            var userSend = await _userRepository.GetUserInfoById(result.SendFromAccountId);
            MessageNotiData message = new MessageNotiData
            {
                Content = result.Content,
                SendDate = result.SendDate,
                SendFromAccountId = result.SendFromAccountId,
                SendFromAccountName = userSend.FullName,
                SendFromAccountAvatarUrl = _imageRepository.GenerateV4SignedReadUrl(userSend.Avatar?.FileName)
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
            var data = JsonConvert.SerializeObject(message, settings);
            if (tokens.Count > 0)
            {


                var responses = await _firebaseSerivce.SendThanksMessage(tokens, data);

                _firebaseTokenRepository.CleanExpiredToken(tokens, responses);
            }
            await _notificationRepository.AddAsync(new Notification
            {
                Type = "5",
                Data = data,
                UserId = result.SendToAccountId,
                CreateTime = DateTime.UtcNow
            });
            receiveRequest.Thanks = request.thanks;

            await _receiveItemRepository.UpdateAsync(receiveRequest);
            return new Response<int>("Update successfully");
        }
    }
}
