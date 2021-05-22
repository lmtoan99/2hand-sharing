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
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IMapper _mapper;
        public SendMessageCommandHandler(IMessageRepositoryAsync messageRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, IFirebaseSerivce firebaseSerivce, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _firebaseSerivce = firebaseSerivce;
            _mapper = mapper;
        }
        public async Task<Response<MessageDTO>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var result = new Domain.Entities.Message {
                Content = request.Content,
                SendFromAccountId = request.SendFromAccountId,
                SendToAccountId = request.SendToAccountId,
                SendDate = DateTime.Now.ToUniversalTime()
            };

            var list_firebase = await _firebaseTokenRepository.GetListFirebaseToken(request.SendToAccountId);
            
            _firebaseSerivce.SendMessage(list_firebase, request.Content);

            return new Response<MessageDTO>(_mapper.Map<MessageDTO>(result));
        }
    }
}
