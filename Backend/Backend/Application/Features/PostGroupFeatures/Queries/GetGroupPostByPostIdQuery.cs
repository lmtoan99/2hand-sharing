using Application.DTOs.GroupPost;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PostGroupFeatures.Queries
{
    public class GetGroupPostByPostIdQuery : IRequest<Response<GetGroupPostByIdViewModel>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
    public class GetGroupPostByPostIdQueryHandler : IRequestHandler<GetGroupPostByPostIdQuery, Response<GetGroupPostByIdViewModel>>
    {
        private readonly IGroupPostRepositoryAsync _groupPostRepository;
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public GetGroupPostByPostIdQueryHandler(IGroupPostRepositoryAsync groupPostRepository,
            IGroupRepositoryAsync groupRepository,
            IGroupAdminDetailRepositoryAsync groupAdminDetailRepository,
            IGroupMemberDetailRepositoryAsync groupMemberDetailRepository,
            IImageRepository imageRepository, IMapper mapper)
        {
            _groupPostRepository = groupPostRepository;
            _groupRepository = groupRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<GetGroupPostByIdViewModel>> Handle(GetGroupPostByPostIdQuery query, CancellationToken cancellationToken)
        {
            var groupPost = await _groupPostRepository.GetByIdAsync(query.PostId);
            if (groupPost == null) throw new KeyNotFoundException($"PostId Not Found.");

            var checkAdmin = await _groupAdminDetailRepository.GetByConditionAsync(e => e.AdminId == query.UserId && e.GroupId == groupPost.GroupId);
            var checkMember = await _groupMemberDetailRepository.GetByConditionAsync(e => e.MemberId == query.UserId && e.GroupId == groupPost.GroupId);

            if (checkAdmin.Count > 0 || checkMember.Count > 0)
            {
                var groupPostViewModel = _mapper.Map<GetGroupPostByIdViewModel>(groupPost);
                groupPostViewModel.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(groupPostViewModel.AvatarUrl);
                for (int i = 0; i < groupPostViewModel.ImageUrl.Count; i++)
                {
                    groupPostViewModel.ImageUrl[i] = _imageRepository.GenerateV4SignedReadUrl(groupPostViewModel.ImageUrl[i]);
                }

                return new Response<GetGroupPostByIdViewModel>(groupPostViewModel);
            }

            throw new UnauthorizedAccessException("You are not member in this group.");
        }
    }
}
