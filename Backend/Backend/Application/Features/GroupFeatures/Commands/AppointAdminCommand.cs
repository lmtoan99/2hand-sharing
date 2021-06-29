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
    public class AppointAdminCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public int MemberId { get; set; }
        public int GroupId { get; set; }
    }
    public class AppointAdminCommandHandler : IRequestHandler<AppointAdminCommand, Response<string>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepositoryAsync;
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;

        public AppointAdminCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepositoryAsync, IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync)
        {
            _groupAdminDetailRepositoryAsync = groupAdminDetailRepositoryAsync;
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
        }

        public async Task<Response<string>> Handle(AppointAdminCommand request, CancellationToken cancellationToken)
        {
            var admin = await _groupAdminDetailRepositoryAsync.GetByConditionAsync(a => a.AdminId == request.UserId && a.GroupId == request.GroupId);
            if(admin.Count == 0)
            {
                throw new ApiException("You are not the admin in this group");
            }
            var member = await _groupMemberDetailRepositoryAsync.GetByConditionAsync(m => m.GroupId == request.GroupId && m.MemberId == request.MemberId && m.JoinStatus == (int) MemberJoinStatus.ACCEPTED);
            if(member.Count == 0)
            {
                throw new ApiException("Member not exists");
            }
            await _groupAdminDetailRepositoryAsync.AddAsync(new GroupAdminDetail { AdminId = request.MemberId, AppointDate = DateTime.Now, GroupId = request.GroupId });
            await _groupMemberDetailRepositoryAsync.DeleteAsync(member[0]);
            return new Response<string>(null);
        }
    }
}
