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
    public class GetAllGroupJoinedQuery : IRequest<Response<IEnumerable<GroupJoinedViewModel>>>
    {
        public int UserId { get; set; }
    }
    public class GetAllGroupJoinedQueryHandler : IRequestHandler<GetAllGroupJoinedQuery, Response<IEnumerable<GroupJoinedViewModel>>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupJoinedQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync, IMapper mapper, IImageRepository imageRepository)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GroupJoinedViewModel>>> Handle(GetAllGroupJoinedQuery request, CancellationToken cancellationToken)
        {
            var res = await _groupMemberDetailRepositoryAsync.GetAllGroupJoinedByUserIdAsync(request.UserId);
            List<GroupJoinedViewModel> list = _mapper.Map<List<GroupJoinedViewModel>>(res);
            return new Response<IEnumerable<GroupJoinedViewModel>>(list);
        }
    }
}
