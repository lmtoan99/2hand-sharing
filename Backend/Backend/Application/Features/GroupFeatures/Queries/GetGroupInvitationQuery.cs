using Application.DTOs.Group;
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

namespace Application.Features.GroupFeatures.Queries
{
    public class GetGroupInvitationQuery : RequestParameter, IRequest<PagedResponse<List<Invitation>>>
    {
        public int UserId;
    }
    public class GetGroupInvitationQueryHandler : IRequestHandler<GetGroupInvitationQuery, PagedResponse<List<Invitation>>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public GetGroupInvitationQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<Invitation>>> Handle(GetGroupInvitationQuery request, CancellationToken cancellationToken)
        {
            var invitations = await _groupMemberDetailRepository.GetInvitationListByUserIdAsync(request.PageNumber, request.PageSize, request.UserId);
            var value = _mapper.Map<List<Invitation>>(invitations);
            value.ForEach(v => v.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(v.AvatarUrl));
            return new PagedResponse<List<Invitation>>(value, request.PageNumber,request.PageSize);
        }
    }
}
