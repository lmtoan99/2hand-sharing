using Application.DTOs.Comment;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CommentFeatures.Commands
{
    public class UpdateCommentCommand : PostCommentOnGroupDTO, IRequest<Response<CommentDTO>>
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Response<CommentDTO>>
    {
        private readonly ICommentRepositoryAsync _commentRepository;
        private readonly IMapper _mapper;
        public UpdateCommentCommandHandler(ICommentRepositoryAsync commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<Response<CommentDTO>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment == null)
            {
                throw new KeyNotFoundException("CommentId not found");
            }
            if (comment.PostByAccontId != request.UserId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            comment.Content = request.Content;
            await _commentRepository.UpdateAsync(comment);
            return new Response<CommentDTO>(_mapper.Map<CommentDTO>(comment));
        }
    }
}
