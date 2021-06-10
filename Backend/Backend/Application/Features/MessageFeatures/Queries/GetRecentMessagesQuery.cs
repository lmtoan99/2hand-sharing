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
        private readonly IImageRepository _imageRepository;
        public GetRecentMessagesQueryHandler(IMessageRepositoryAsync messageRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        public async Task<PagedResponse<IReadOnlyList<RecentMessagesDTO>>> Handle(GetRecentMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetRecentMessages(request.UserId, request.PageNumber, request.PageSize * 2);
            var messagesDTOs = _mapper.Map<List<RecentMessagesDTO>>(messages);

            foreach (var i in messagesDTOs)
            {
                i.AvatarUrlSendFromAccount = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrlSendFromAccount);
                i.AvatarUrlSendToAccount = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrlSendToAccount);
            }
            for (int i = 0; i < messagesDTOs.Count - 1; i++)
            {
                for (int j = i + 1; j < messagesDTOs.Count; j++)
                {
                    if (messagesDTOs[i].SendFromAccountId == messagesDTOs[j].SendToAccountId && messagesDTOs[i].SendToAccountId == messagesDTOs[j].SendFromAccountId)
                    {
                        if (messagesDTOs[i].SendDate.CompareTo(messagesDTOs[j].SendDate) > 0)
                        {
                            messagesDTOs.RemoveAt(j);
                        }
                        else
                        {
                            messagesDTOs.RemoveAt(i);
                            i--;
                        }
                        break;
                    }
                }
            }
            return new PagedResponse<IReadOnlyList<RecentMessagesDTO>>(messagesDTOs, request.PageNumber, request.PageSize);
        }
    }
}
