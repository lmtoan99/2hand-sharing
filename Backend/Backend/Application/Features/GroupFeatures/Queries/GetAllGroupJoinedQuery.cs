using Application.DTOs.Group;
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
    public class GetAllGroupJoinedQuery : IRequest<Response<IEnumerable<GroupViewModel>>>
    {
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllGroupJoinedQueryHandler : IRequestHandler<GetAllGroupJoinedQuery, Response<IEnumerable<GroupViewModel>>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupJoinedQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync, IMapper mapper, IImageRepository imageRepository)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GroupViewModel>>> Handle(GetAllGroupJoinedQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupJoinedParameter>(request);
            var res = await _groupMemberDetailRepositoryAsync.GetAllGroupJoinedByUserIdAsync(validFilter.PageNumber, validFilter.PageSize, request.UserId);
            List<GroupViewModel> list = _mapper.Map<List<GroupViewModel>>(res);
            return new Response<IEnumerable<GroupViewModel>>(list);
        }
    }
}
