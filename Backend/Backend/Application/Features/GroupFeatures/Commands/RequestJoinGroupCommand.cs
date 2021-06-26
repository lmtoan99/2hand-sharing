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
    public class RequestJoinGroupCommand : IRequest<Response<string>>
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
    public class RequestJoinGroupCommandHandler : IRequestHandler<RequestJoinGroupCommand, Response<string>>
    {
        public IGroupMemberDetailRepositoryAsync _groupMemberDetail;
        public RequestJoinGroupCommandHandler(IGroupMemberDetailRepositoryAsync groupMemberDetail)
        {
            _groupMemberDetail = groupMemberDetail;
        }
        public async Task<Response<string>> Handle(RequestJoinGroupCommand request, CancellationToken cancellationToken)
        {
            var checkExist = await _groupMemberDetail.GetMemberGroup(request.GroupId, request.UserId);
            if (checkExist != null)
            {
                throw new ApiException("User joined this group");
            }
            var entity = new GroupMemberDetail{
                GroupId = request.GroupId,
                JoinDate = DateTime.UtcNow,
                MemberId = request.UserId,
                JoinStatus = (int)MemberJoinStatus.JOIN_REQUEST
            };
            await _groupMemberDetail.AddAsync(entity);
            return new Response<string>("Success", null);
        }
    }
}
