using Application.Enums;
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
    public class GetRoleMemberInGroupQuery : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
    public class GetRoleMemberInGroupQueryHandler : IRequestHandler<GetRoleMemberInGroupQuery, Response<string>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepositoryAsync;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;
        public GetRoleMemberInGroupQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
            _groupAdminDetailRepositoryAsync = groupAdminDetailRepository;
        }
        public async Task<Response<string>> Handle(GetRoleMemberInGroupQuery request, CancellationToken cancellationToken)
        {
            var admin = await _groupAdminDetailRepositoryAsync.GetByConditionAsync(i => i.GroupId==request.GroupId && i.AdminId==request.UserId);

            if (admin.Count != 0)
            {
                return new Response<string>("admin");
            }
            var member = await _groupMemberDetailRepositoryAsync.GetByConditionAsync(i => i.GroupId == request.GroupId && i.MemberId == request.UserId);
            if (member.Count != 0)
            {
                
                if(member[0].JoinStatus == (int) MemberJoinStatus.ACCEPTED)
                return new Response<string>("member");
            }

            throw new ApiException("Member not in group.");

        }
    }
}
