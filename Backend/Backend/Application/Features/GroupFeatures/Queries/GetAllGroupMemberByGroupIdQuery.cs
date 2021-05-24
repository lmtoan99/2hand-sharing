using Application.DTOs.Group;
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

namespace Application.Features.GroupFeatures.Queries
{
    public class GetAllGroupMemberByGroupIdQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupMemberViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int GroupId { get; set; }
    }
    public class GetAllGroupMemberByGroupIdQueryHandler : IRequestHandler<GetAllGroupMemberByGroupIdQuery, PagedResponse<IEnumerable<GetAllGroupMemberViewModel>>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IMapper _mapper;
        public GetAllGroupMemberByGroupIdQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IMapper mapper)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllGroupMemberViewModel>>> Handle(GetAllGroupMemberByGroupIdQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupMemberByGroupIdParameter>(request);
            var groupMember = await _groupMemberDetailRepository.GetAllGroupMemberByGroupIdAsync(validFilter.PageNumber, validFilter.PageSize, request.GroupId);
            var groupMemberViewModel = _mapper.Map<List<GetAllGroupMemberViewModel>>(groupMember);

            return new PagedResponse<IEnumerable<GetAllGroupMemberViewModel>>(groupMemberViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
