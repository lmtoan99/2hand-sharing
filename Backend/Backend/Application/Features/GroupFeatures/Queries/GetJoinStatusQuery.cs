using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Queries
{
    public class GetJoinStatusQuery : IRequest<Response<int>>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
    public class GetJoinStatusQueryHandler : IRequestHandler<GetJoinStatusQuery, Response<int>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepositoryAsync;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;
        public GetJoinStatusQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
            _groupAdminDetailRepositoryAsync = groupAdminDetailRepository;
        }

        public async Task<Response<int>> Handle(GetJoinStatusQuery request, CancellationToken cancellationToken)
        {
            var admin = await _groupAdminDetailRepositoryAsync.GetByConditionAsync(a => a.AdminId == request.UserId && a.GroupId == request.GroupId);
            if(admin.Count > 0)
            {
                return new Response<int>(4);
            }
            var member = await _groupMemberDetailRepositoryAsync.GetByConditionAsync(a => a.GroupId == request.GroupId && a.MemberId == request.UserId);
            if(member.Count > 0)
            {
                return new Response<int>(member[0].JoinStatus);
            }
            return new Response<int>(0);
        }
    }
}
