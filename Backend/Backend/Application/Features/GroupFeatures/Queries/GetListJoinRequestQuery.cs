using Application.DTOs.Group;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Queries
{
    public class GetListJoinRequestQuery : IRequest<Response<IEnumerable<MemberJoinedRequestViewModel>>>
    {
        public int AdminId { get; set; }
        public int GroupId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetListJoinRequestQueryHandler : IRequestHandler<GetListJoinRequestQuery, Response<IEnumerable<MemberJoinedRequestViewModel>>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepositoryAsync;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetListJoinRequestQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync, IGroupAdminDetailRepositoryAsync groupAdminDetailRepositoryAsync,IMapper mapper, IImageRepository imageRepository)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
            _groupAdminDetailRepositoryAsync = groupAdminDetailRepositoryAsync;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<MemberJoinedRequestViewModel>>> Handle(GetListJoinRequestQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetListJoinGroupRequestParameter>(request);
            var requestJoin = await _groupMemberDetailRepositoryAsync.GetListJoinGroupRequestByGroupIdAsync(validFilter.PageNumber, validFilter.PageSize, request.GroupId);
            var groupAdminInfo = await _groupAdminDetailRepositoryAsync.GetInfoGroupAdminDetail(request.GroupId, request.AdminId);
            if (groupAdminInfo == null)
            {
                throw new ApiException("You are not admin of this group.");
            }

            var response = _mapper.Map<IEnumerable<MemberJoinedRequestViewModel>>(requestJoin);
            foreach (MemberJoinedRequestViewModel i in response)
            {
                i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
            }
            return new Response<IEnumerable<MemberJoinedRequestViewModel>>(response);
        }
    }
}

