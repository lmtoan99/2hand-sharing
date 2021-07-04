using Application.DTOs.Comment;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupPostFeatures.Commands
{
    public class PostCommentOnGroupPostCommand : PostCommentOnGroupDTO, IRequest<Response<CommentDTO>>
    {
        public int PostId { get; set; }
        public int PostByAccontId { get; set; }
    }
    public class PostCommentOnGroupPostCommandHandler : IRequestHandler<PostCommentOnGroupPostCommand, Response<CommentDTO>>
    {
        private readonly IGroupPostRepositoryAsync _groupPostRepository;
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly ICommentRepositoryAsync _commentRepository;
        private readonly IMapper _mapper;
        public PostCommentOnGroupPostCommandHandler(IGroupPostRepositoryAsync groupPostRepository, IGroupRepositoryAsync groupRepository, ICommentRepositoryAsync commentRepository, IMapper mapper)
        {
            _groupPostRepository = groupPostRepository;
            _groupRepository = groupRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<Response<CommentDTO>> Handle(PostCommentOnGroupPostCommand request, CancellationToken cancellationToken)
        {
            var post = await _groupPostRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("PostId not found");
            }
            var result = await _groupRepository.CheckUserInGroup(post.GroupId, request.PostByAccontId);
            if (!result)
            {
                throw new UnauthorizedAccessException("User can not comment on this post");
            }
            var newCmt = new Comment { PostId = post.Id, PostByAccontId = request.PostByAccontId, Content = request.Content, PostTime = DateTime.UtcNow };
            newCmt = await _commentRepository.AddAsync(newCmt);
            return new Response<CommentDTO>(_mapper.Map<CommentDTO>(newCmt));
        }
    }
}
