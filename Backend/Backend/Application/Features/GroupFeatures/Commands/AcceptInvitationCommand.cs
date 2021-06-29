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

namespace Application.Features.GroupFeatures.Commands
{
    public class AcceptInvitationCommand : IRequest<Response<string>>
    {
        public int UserId;
        public int GroupId;
    }
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Response<string>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;

        public AcceptInvitationCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepositoryAsync, IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
        }

        public async Task<Response<string>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            var member = await _groupMemberDetailRepositoryAsync.GetByConditionAsync(m => m.GroupId == request.GroupId && m.MemberId == request.UserId && m.JoinStatus == (int)MemberJoinStatus.ADMIN_INVITE);
            if (member.Count == 0)
            {
                throw new ApiException("Invitation not exists");
            }
            member[0].JoinStatus = (int)MemberJoinStatus.ACCEPTED;
            await _groupMemberDetailRepositoryAsync.UpdateAsync(member[0]);
            return new Response<string>(null);
        }
    }
}
