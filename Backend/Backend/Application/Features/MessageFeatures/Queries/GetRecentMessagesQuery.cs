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
    public class GetRecentMessagesQuery : IRequest<PagedResponse<IReadOnlyList<RecentMessagesDTO>>>
    {
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetRecentMessagesQueryHandler : IRequestHandler<GetRecentMessagesQuery, PagedResponse<IReadOnlyList<RecentMessagesDTO>>>
    {
        private readonly IMessageRepositoryAsync _messageRepository;
        private readonly IMapper _mapper;
        public GetRecentMessagesQueryHandler(IMessageRepositoryAsync messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IReadOnlyList<RecentMessagesDTO>>> Handle(GetRecentMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetRecentMessages(request.UserId, request.PageNumber, request.PageSize * 2);
            var messagesDTOs = _mapper.Map<IReadOnlyList<RecentMessagesDTO>>(messages);
            var results = new List<RecentMessagesDTO>();
            for (int i = 0; i < messagesDTOs.Count; i++)
            {
                for(int j = i + 1; j < messagesDTOs.Count - 1; j++)
                {
                    if(messagesDTOs[i].SendFromAccountId == messagesDTOs[j].SendToAccountId && messagesDTOs[i].SendToAccountId == messagesDTOs[j].SendFromAccountId)
                    {
                        if(messagesDTOs[i].SendDate > messagesDTOs[j].SendDate)
                        {
                            results.Add(messagesDTOs[i]);
                        }
                    }
                }
            }
            return new PagedResponse<IReadOnlyList<RecentMessagesDTO>>(results, request.PageNumber, request.PageSize);
        }
    }
}
