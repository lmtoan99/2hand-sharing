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
    public class DeleteMemberCommand : IRequest<Response<int>>
    {
        public int groupId { get; set; }
        public int memberId { get; set; }
        public int adminId { get; set; }
    }
    public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Response<int>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        public DeleteMemberCommandHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
        }
        public async Task<Response<int>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            var checkAdmin = await _groupAdminDetailRepository.GetByConditionAsync(g => g.AdminId == request.adminId && g.GroupId == request.groupId);
            if (checkAdmin.Count == 0) {
                throw new UnauthorizedAccessException("User does not have permission to do this action");
            }
            var detail = await _groupMemberDetailRepository.GetByConditionAsync(d => d.GroupId == request.groupId && d.MemberId == request.memberId);
            if (detail.Count == 0)
            {
                throw new KeyNotFoundException("Member not found");
            }
            await _groupMemberDetailRepository.DeleteAsync(detail[0]);
            return new Response<int>(0);
        }
    }
}
