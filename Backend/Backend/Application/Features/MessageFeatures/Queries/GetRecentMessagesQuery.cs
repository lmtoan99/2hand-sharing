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
    public class GetRecentMessagesQuery : IRequest<PagedResponse<IReadOnlyCollection<RecentMessagesDTO>>>
    {
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetRecentMessagesQueryHandler : IRequestHandler<GetRecentMessagesQuery, PagedResponse<IReadOnlyCollection<RecentMessagesDTO>>>
    {
        private readonly IMessageRepositoryAsync _messageRepository;
        private readonly IMapper _mapper;
        public GetRecentMessagesQueryHandler(IMessageRepositoryAsync messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IReadOnlyCollection<RecentMessagesDTO>>> Handle(GetRecentMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetRecentMessages(request.UserId, request.PageNumber, request.PageSize);
            return new PagedResponse<IReadOnlyCollection<RecentMessagesDTO>>(_mapper.Map<IReadOnlyCollection<RecentMessagesDTO>>(messages), request.PageNumber, request.PageSize);
        }
    }
}
