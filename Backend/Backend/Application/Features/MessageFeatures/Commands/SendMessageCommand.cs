using Application.DTOs.Message;
using Application.Interfaces.Repositories;
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
        private readonly IMapper _mapper;
        public SendMessageCommandHandler(IMessageRepositoryAsync messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
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

            return new Response<MessageDTO>(_mapper.Map<MessageDTO>(result));
        }
    }
}
