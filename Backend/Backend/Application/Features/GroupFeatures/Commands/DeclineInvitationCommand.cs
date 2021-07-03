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
    public class DeclineInvitationCommand : IRequest<Response<string>>
    {
        public int UserId;
        public int GroupId;
    }
    public class DeclineInvitationCommandHandler : IRequestHandler<DeclineInvitationCommand, Response<string>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;

        public DeclineInvitationCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepositoryAsync, IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
        }

        public async Task<Response<string>> Handle(DeclineInvitationCommand request, CancellationToken cancellationToken)
        {
            var member = await _groupMemberDetailRepositoryAsync.GetByConditionAsync(m => m.GroupId == request.GroupId && m.MemberId == request.UserId && m.JoinStatus == (int)MemberJoinStatus.ADMIN_INVITE);
            if (member.Count == 0)
            {
                throw new ApiException("Invitation not exists");
            }
            await _groupMemberDetailRepositoryAsync.DeleteAsync(member[0]);
            return new Response<string>(null);
        }
    }

}
