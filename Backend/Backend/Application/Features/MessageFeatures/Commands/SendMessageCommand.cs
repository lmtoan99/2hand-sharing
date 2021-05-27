using Application.DTOs.Firebase;
using Application.DTOs.Message;
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
        public SendMessageCommandHandler(IMessageRepositoryAsync messageRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, IUserRepositoryAsync userRepository, IFirebaseSerivce firebaseSerivce, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _userRepository = userRepository;
            _firebaseSerivce = firebaseSerivce;
            _mapper = mapper;
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
                string SenderName = await _userRepository.GetUserFullnameById(result.SendFromAccountId);
                MessageNotiData message = new MessageNotiData
                {
                    Content = result.Content,
                    SendDate = result.SendDate,
                    SendFromAccountId = result.SendFromAccountId,
                    SendFromAccountName = SenderName
                };
                var responses = await _firebaseSerivce.SendMessage(tokens, message);
                _firebaseTokenRepository.CleanExpiredToken(tokens, responses);
            }
            return new Response<MessageDTO>(_mapper.Map<MessageDTO>(result));
        }
    }
}
