using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Commands
{
    public class AcceptJoinRequestCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public int MemberId { get; set; }
        public int GroupId { get; set; }
    }
    public class AcceptJoinRequestCommandHandler : IRequestHandler<AcceptJoinRequestCommand, Response<string>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepositoryAsync;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;

        public AcceptJoinRequestCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepositoryAsync, IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync)
        {
            _groupAdminDetailRepositoryAsync = groupAdminDetailRepositoryAsync;
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
        }

        public async Task<Response<string>> Handle(AcceptJoinRequestCommand request, CancellationToken cancellationToken)
        {
            var admin = await _groupAdminDetailRepositoryAsync.GetByConditionAsync(a => a.AdminId == request.UserId && a.GroupId == request.GroupId);
            if (admin.Count == 0)
            {
                throw new ApiException("You are not the admin in this group");
            }
            var member = await _groupMemberDetailRepositoryAsync.GetByConditionAsync(m => m.GroupId == request.GroupId && m.MemberId == request.MemberId && m.JoinStatus == (int) MemberJoinStatus.JOIN_REQUEST);
            if (member.Count == 0)
            {
                throw new ApiException("Request not exists");
            }
            member[0].JoinStatus = (int)MemberJoinStatus.ACCEPTED;
            await _groupMemberDetailRepositoryAsync.UpdateAsync(member[0]);
            return new Response<string>(null);
        }
    }
}
