using Application.DTOs.Comment;
using Application.Exceptions;
using Application.Filter;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupPostFeatures.Queries
{
    public class GetListCommentOnPostQuery : RequestParameter, IRequest<PagedResponse<IReadOnlyCollection<ListCommentDTO>>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
    public class GetListCommentOnPostQueryHandler : IRequestHandler<GetListCommentOnPostQuery, PagedResponse<IReadOnlyCollection<ListCommentDTO>>>
    {
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IGroupPostRepositoryAsync _groupPostRepository;
        private readonly ICommentRepositoryAsync _commentRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetListCommentOnPostQueryHandler(IGroupRepositoryAsync groupRepository, IGroupPostRepositoryAsync groupPostRepository, ICommentRepositoryAsync commentRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _groupPostRepository = groupPostRepository;
            _commentRepository = commentRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyCollection<ListCommentDTO>>> Handle(GetListCommentOnPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _groupPostRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("PostId not found");
            }
            var checkPermission = await _groupRepository.CheckUserInGroup(post.GroupId, request.UserId);
            if (!checkPermission)
            {
                throw new UnauthorizedAccessException("User not in group");
            }
            var result = await _commentRepository.GetListCommentByPostId(request.PostId, request.PageNumber, request.PageSize);
            var value = _mapper.Map<List<ListCommentDTO>>(result);
            value.ForEach(c => c.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(c.AvatarUrl));
            return new PagedResponse<IReadOnlyCollection<ListCommentDTO>>(value, request.PageNumber, request.PageSize);
        }
    }
}
