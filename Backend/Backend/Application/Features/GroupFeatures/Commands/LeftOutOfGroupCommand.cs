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
    public class LeftOutOfGroupCommand : IRequest<Response<string>>
    {
        public int groupId { get; set; }
        public int memberId { get; set; }
    }
    public class LeftOutOfGroupCommandHandler : IRequestHandler<LeftOutOfGroupCommand, Response<string>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        public LeftOutOfGroupCommandHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
        }
        public async Task<Response<string>> Handle(LeftOutOfGroupCommand request, CancellationToken cancellationToken)
        {
            var checkAdmin = await _groupAdminDetailRepository.GetByConditionAsync(e => e.GroupId == request.groupId && e.AdminId == request.memberId);
            if (checkAdmin.Count == 1)
            {
                var adminGroup = await _groupAdminDetailRepository.GetByConditionAsync(e => e.GroupId == request.groupId);
                if (adminGroup.Count == 1)
                {
                    return new Response<string>("There is only one admin in this group.");
                }
                else
                {
                    await _groupAdminDetailRepository.DeleteAsync(checkAdmin[0]);
                    return new Response<string>("ok");
                }  
            }

            var checkMember = await _groupMemberDetailRepository.GetByConditionAsync(e => e.GroupId == request.groupId && e.MemberId == request.memberId);
            if (checkMember == null)
            {
                throw new KeyNotFoundException("Group member not found.");
            }
            else
            {
                await _groupMemberDetailRepository.DeleteAsync(checkMember[0]);
                return new Response<string>("ok");
            }      
        }
    }
}
