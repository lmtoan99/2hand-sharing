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

namespace Application.Features.MessageFeatures.Queries
{
    public class GetMessageQuery : IRequest<PagedResponse<IReadOnlyCollection<MessageDTO>>>
    {
        public int UserGetMessageId { get; set; }
        public int UserMessageWithId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, PagedResponse<IReadOnlyCollection<MessageDTO>>>
    {
        private readonly IMessageRepositoryAsync _messageRepository;
        private readonly IMapper _mapper;
        public GetMessageQueryHandler(IMessageRepositoryAsync messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyCollection<MessageDTO>>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageRepository.GetListMessage(request.UserGetMessageId, request.UserMessageWithId, request.PageNumber, request.PageSize);

            return new PagedResponse<IReadOnlyCollection<MessageDTO>>(_mapper.Map<IReadOnlyCollection<MessageDTO>>(result), request.PageNumber, request.PageSize);
        }
    }
}
