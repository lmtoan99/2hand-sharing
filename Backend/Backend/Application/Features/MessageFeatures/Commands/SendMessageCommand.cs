using Application.DTOs.Firebase;
using Application.DTOs.Message;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.MessageFeatures.Commands
{
    public class SendMessageCommand : IRequest<Response<MessageDTO>>
    {
        public string Content { get; set; }
        public int SendFromAccountId { get; set; }
        public int SendToAccountId { get; set; }
    }
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Response<MessageDTO>>
    {
        private readonly IMessageRepositoryAsync _messageRepository;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;
        public SendMessageCommandHandler(IMessageRepositoryAsync messageRepository, 
            IFirebaseTokenRepositoryAsync firebaseTokenRepository, 
            IUserRepositoryAsync userRepository, 
            IFirebaseSerivce firebaseSerivce, 
            IMapper mapper,
            IImageRepository imageRepository)
        {
            _messageRepository = messageRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _userRepository = userRepository;
            _firebaseSerivce = firebaseSerivce;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }
        public async Task<Response<MessageDTO>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var result = await _messageRepository.AddAsync(new Domain.Entities.Message {
                Content = request.Content,
                SendFromAccountId = request.SendFromAccountId,
                SendToAccountId = request.SendToAccountId,
                SendDate = DateTime.Now.ToUniversalTime()
            });
            
            var tokens = await _firebaseTokenRepository.GetListFirebaseToken(request.SendToAccountId);
            if (tokens.Count > 0)
            {
                var user = await _userRepository.GetUserInfoById(result.SendFromAccountId);
                MessageNotiData message = new MessageNotiData
                {
                    Content = result.Content,
                    SendDate = result.SendDate,
                    SendFromAccountId = result.SendFromAccountId,
                    SendFromAccountName = user.FullName,
                    SendFromAccountAvatarUrl = _imageRepository.GenerateV4SignedReadUrl(user.Avatar?.FileName)
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
                var responses = await _firebaseSerivce.SendMessage(tokens, JsonConvert.SerializeObject(message, settings));
                _firebaseTokenRepository.CleanExpiredToken(tokens, responses);
            }
            return new Response<MessageDTO>(_mapper.Map<MessageDTO>(result));
        }
    }
}
