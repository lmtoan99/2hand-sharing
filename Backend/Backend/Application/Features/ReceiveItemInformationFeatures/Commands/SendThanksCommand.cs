using Application.DTOs.Firebase;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
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
        private readonly IMessageRepositoryAsync _messageRepository;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;

        public SendThanksCommandHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository, IMessageRepositoryAsync messageRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, IUserRepositoryAsync userRepository, IFirebaseSerivce firebaseSerivce)
        {
            _receiveItemRepository = receiveItemRepository;
            _messageRepository = messageRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _userRepository = userRepository;
            _firebaseSerivce = firebaseSerivce;
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
            if (tokens.Count > 0)
            {
                string SenderName = await _userRepository.GetUserFullnameById(result.SendFromAccountId);
                MessageNotiData message = new MessageNotiData
                {
                    Content = result.Content,
                    SendDate = result.SendDate,
                    SendFromAccountId = result.SendFromAccountId,
                    SendFromAccountName = SenderName
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
                var responses = await _firebaseSerivce.SendThanksMessage(tokens, JsonConvert.SerializeObject(message, settings));
                _firebaseTokenRepository.CleanExpiredToken(tokens, responses);
            }
            receiveRequest.Thanks = request.thanks;
            await _receiveItemRepository.UpdateAsync(receiveRequest);
            return new Response<int>("Update successfully");
        }
    }
}
