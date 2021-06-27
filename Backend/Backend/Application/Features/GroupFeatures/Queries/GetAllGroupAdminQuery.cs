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
    public class GetAllGroupAdminQuery : RequestParameter ,IRequest<PagedResponse<List<GetAllGroupMemberViewModel>>>
    {
        public int GroupId;
    }
    public class GetAllGroupAdminQueryHandler : IRequestHandler<GetAllGroupAdminQuery, PagedResponse<List<GetAllGroupMemberViewModel>>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetail;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetAllGroupAdminQueryHandler(IGroupAdminDetailRepositoryAsync groupAdminDetail, IImageRepository imageRepository, IMapper mapper)
        {
            _groupAdminDetail = groupAdminDetail;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }
        public async Task<PagedResponse<List<GetAllGroupMemberViewModel>>> Handle(GetAllGroupAdminQuery request, CancellationToken cancellationToken)
        {
            var result = await _groupAdminDetail.GetListAdminByGroupId(request.GroupId, request.PageNumber, request.PageSize);
            var value = _mapper.Map<List<GetAllGroupMemberViewModel>>(result);
            value.ForEach(v => v.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(v.AvatarUrl));

            return new PagedResponse<List<GetAllGroupMemberViewModel>>(value,request.PageNumber, request.PageSize);
        }
    }
}
