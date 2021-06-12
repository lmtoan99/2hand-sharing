using Application.DTOs.Group;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Commands
{

    public class AddMemberCommand : IRequest<Response<GroupMemberDTO>>
    {
        public string Email { get; set; }
        public int GroupId { get; set; }
        public int AdminId { get; set; }
    }

    class AddMemberCommandHandle : IRequestHandler<AddMemberCommand, Response<GroupMemberDTO>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IAccountService _accountService;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IMapper _mapper;
        public AddMemberCommandHandle(IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IAccountService accountService, IUserRepositoryAsync userRepositoryAsync, IMapper mapper)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _accountService = accountService;
            _userRepositoryAsync = userRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<GroupMemberDTO>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var accountId = await _accountService.GetAcccountIdByEmail(request.Email);
            if (accountId == null)
            {
                throw new ApiException("Email does not exist.");
            }
            var user = await _userRepositoryAsync.GetUserByAccountId(accountId);
            var groupAdminInfo = await _groupAdminDetailRepository.GetInfoGroupAdminDetail(request.GroupId, request.AdminId);
            if (groupAdminInfo == null)
            {
                throw new ApiException("You are not admin of this group.");
            }
            var checkMemberInGroup = await _groupMemberDetailRepository.GetMemberGroup(request.GroupId, user.Id);
            if (checkMemberInGroup==null)
            {
                GroupMemberDetail groupMember = new GroupMemberDetail
                {
                    MemberId = user.Id,
                    GroupId = request.GroupId,
                    ReportStatus = true,
                    JoinDate = DateTime.Now.ToUniversalTime()
                };

                var result = await _groupMemberDetailRepository.AddAsync(groupMember);
                return new Response<GroupMemberDTO>(_mapper.Map<GroupMemberDTO>(result));
            }
            else
            {
                throw new ApiException("Member exist in group.");
            }
        }
    }
}
