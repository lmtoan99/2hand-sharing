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

    public class CancelJoinRequestCommand : IRequest<Response<string>>
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
    public class CancelJoinRequestCommandHandler : IRequestHandler<CancelJoinRequestCommand, Response<string>>
    {
        public IGroupMemberDetailRepositoryAsync _groupMemberDetail;
        public CancelJoinRequestCommandHandler(IGroupMemberDetailRepositoryAsync groupMemberDetail)
        {
            _groupMemberDetail = groupMemberDetail;
        }
        public async Task<Response<string>> Handle(CancelJoinRequestCommand request, CancellationToken cancellationToken)
        {
            var joinRequest = await _groupMemberDetail.GetMemberGroup(request.GroupId, request.UserId);
            if (joinRequest == null)
            {
                throw new ApiException("There is no request");
            }
            else if (joinRequest.JoinStatus == (int)MemberJoinStatus.ACCEPTED)
            {
                throw new ApiException("Request was accepted");
            }

            await _groupMemberDetail.DeleteAsync(joinRequest);
            return new Response<string>("Success", null);
        }
    }
}
