using Application.DTOs.GroupPost;
using Application.Features.ItemFeatures.Queries;
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
    public class GetAllGroupPostsQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupPostViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int GroupId { get; set; }
        public int PostByAccountId { get; set; }
    }
    public class GetAllItemsQueryHandler : IRequestHandler<GetAllGroupPostsQuery, PagedResponse<IEnumerable<GetAllGroupPostViewModel>>>
    {
        private readonly IGroupPostRepositoryAsync _groupPostRepository;
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetAllItemsQueryHandler(IGroupPostRepositoryAsync groupPostRepository,
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

        public async Task<PagedResponse<IEnumerable<GetAllGroupPostViewModel>>> Handle(GetAllGroupPostsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupPostsParameter>(request);
            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            if (group == null)
            {
                throw new KeyNotFoundException("GroupId not found.");
            }
            var checkAdmin = await _groupAdminDetailRepository.GetByConditionAsync(e => e.AdminId == request.PostByAccountId && e.GroupId == request.GroupId);
            var checkMember = await _groupMemberDetailRepository.GetByConditionAsync(e => e.MemberId == request.PostByAccountId && e.GroupId == request.GroupId);

            if (checkAdmin.Count > 0 || checkMember.Count > 0)
            {
                var groupPosts = await _groupPostRepository.GetAllPublicPostInGroupAsync(request.PageNumber, request.PageSize, request.GroupId);

                List<GetAllGroupPostViewModel> groupPostViewModel = _mapper.Map<List<GetAllGroupPostViewModel>>(groupPosts);
                groupPostViewModel.ForEach(i =>
                {
                    i.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ImageUrl);
                    i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
                });

                return new PagedResponse<IEnumerable<GetAllGroupPostViewModel>>(groupPostViewModel, validFilter.PageNumber, validFilter.PageSize);
            }

            throw new UnauthorizedAccessException("You are not member in this group.");
        }
    }
}
   