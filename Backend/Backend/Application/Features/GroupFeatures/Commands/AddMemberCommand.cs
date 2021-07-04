using Application.DTOs.Group;
using Application.Enums;
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

    public class AddMemberCommand : IRequest<Response<GetAllGroupMemberViewModel>>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int AdminId { get; set; }
    }

    class AddMemberCommandHandle : IRequestHandler<AddMemberCommand, Response<GetAllGroupMemberViewModel>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IMapper _mapper;
        public AddMemberCommandHandle(IImageRepository imageRepository,IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IAccountService accountService, IUserRepositoryAsync userRepositoryAsync, IMapper mapper)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _userRepositoryAsync = userRepositoryAsync;
            _imageRepository =  imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<GetAllGroupMemberViewModel>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepositoryAsync.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ApiException("UserId not exist.");
            }
            var groupAdminInfo = await _groupAdminDetailRepository.GetInfoGroupAdminDetail(request.GroupId, request.AdminId);
            if (groupAdminInfo == null)
            {
                throw new ApiException("You are not admin of this group.");
            }
            var admin = await _groupAdminDetailRepository.GetInfoGroupAdminDetail(request.GroupId, request.UserId);
            if (admin != null)
            {
                throw new ApiException("Member exist in group.");
            }
            var checkMemberInGroup = await _groupMemberDetailRepository.GetMemberGroup(request.GroupId, request.UserId);
            if (checkMemberInGroup == null)
            {
                GroupMemberDetail groupMember = new GroupMemberDetail
                {
                    MemberId = user.Id,
                    GroupId = request.GroupId,
                    ReportStatus = true,
                    JoinDate = DateTime.Now.ToUniversalTime(),
                    JoinStatus = (int) MemberJoinStatus.ADMIN_INVITE
                };

                var result = await _groupMemberDetailRepository.AddAsync(groupMember);
                var dto = _mapper.Map<GetAllGroupMemberViewModel>(result);
                dto.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(dto.AvatarUrl);
                return new Response<GetAllGroupMemberViewModel>(null);
            }
            else
            {
                if(checkMemberInGroup.JoinStatus == (int) MemberJoinStatus.JOIN_REQUEST)
                {
                    checkMemberInGroup.JoinStatus = (int) MemberJoinStatus.ACCEPTED;
                    await _groupMemberDetailRepository.UpdateAsync(checkMemberInGroup);
                    var dto = _mapper.Map<GetAllGroupMemberViewModel>(checkMemberInGroup);
                    dto.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(dto.AvatarUrl);
                    return new Response<GetAllGroupMemberViewModel>(dto);
                }
                if (checkMemberInGroup.JoinStatus == (int)MemberJoinStatus.ADMIN_INVITE)
                    throw new ApiException("Member already invited");


            }
            throw new ApiException("Member exist in group.");
        }
    }
}
