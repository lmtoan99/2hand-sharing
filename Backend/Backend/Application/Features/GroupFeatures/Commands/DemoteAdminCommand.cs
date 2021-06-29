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
    public class DemoteAdminCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public int AdminId { get; set; }
        public int GroupId { get; set; }
    }

    public class DemoteAdminCommandHandler : IRequestHandler<DemoteAdminCommand, Response<string>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepositoryAsync;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;

        public DemoteAdminCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepositoryAsync, IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync)
        {
            _groupAdminDetailRepositoryAsync = groupAdminDetailRepositoryAsync;
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
        }

        public async Task<Response<string>> Handle(DemoteAdminCommand request, CancellationToken cancellationToken)
        {
            var admin = await _groupAdminDetailRepositoryAsync.GetByConditionAsync(a => a.AdminId == request.UserId && a.GroupId == request.GroupId);
            if (admin.Count == 0)
            {
                throw new ApiException("You are not the admin in this group");
            }
            var member = await _groupAdminDetailRepositoryAsync.GetByConditionAsync(m => m.GroupId == request.GroupId && m.AdminId == request.AdminId);
            if (member.Count == 0)
            {
                throw new ApiException("Admin not exists");
            }
            await _groupMemberDetailRepositoryAsync.AddAsync(new GroupMemberDetail { MemberId = request.AdminId, JoinDate = DateTime.Now.ToUniversalTime(), GroupId = request.GroupId, ReportStatus = false, JoinStatus = (int) MemberJoinStatus.ACCEPTED} );
            await _groupAdminDetailRepositoryAsync.DeleteAsync(member[0]);
            return new Response<string>(null);
        }
    }
}
