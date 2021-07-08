using Application.DTOs.Firebase;
using Application.DTOs.Group;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly INotificationRepositoryAsync _notificationRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IMapper _mapper;
        private JsonSerializerSettings _settings;

        public AddMemberCommandHandle(IGroupRepositoryAsync groupRepository,IFirebaseSerivce firebaseSerivce,IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IImageRepository imageRepository, IUserRepositoryAsync userRepositoryAsync, IFirebaseTokenRepositoryAsync firebaseTokenRepository, INotificationRepositoryAsync notificationRepository, IMapper mapper)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _imageRepository = imageRepository;
            _userRepositoryAsync = userRepositoryAsync;
            _firebaseTokenRepository = firebaseTokenRepository;
            _notificationRepository = notificationRepository;
            _groupRepository = groupRepository;
            _firebaseSerivce = firebaseSerivce;
            _mapper = mapper;
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            _settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
        }

        public async Task<Response<GetAllGroupMemberViewModel>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            #region Validate
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
            #endregion
            #region InviteMember
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

                await _groupMemberDetailRepository.AddAsync(groupMember);
                var group = await _groupRepository.GetGroupByIdAsync(request.GroupId);
                var data = new InvitationData
                {
                    GroupId = group.Id,
                    GroupName = group.GroupName,
                    AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(group.Avatar?.FileName),
                    InvitationTime = DateTime.UtcNow
                };

                var messageData = JsonConvert.SerializeObject(data, _settings);

                await _notificationRepository.AddAsync(new Notification
                {
                    Type = "7",
                    Data = messageData,
                    UserId = request.UserId,
                    CreateTime = DateTime.UtcNow
                });
                var tokens = await _firebaseTokenRepository.GetListFirebaseToken(request.UserId);
                if (tokens.Count > 0)
                {

                    var responses = await _firebaseSerivce.SendMessage(tokens, messageData, NotificationType.INVITE_MEMBER);
                    _firebaseTokenRepository.CleanExpiredToken(tokens, responses);

                }
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
            #endregion



            throw new ApiException("Member exist in group.");
        }
    }
}
