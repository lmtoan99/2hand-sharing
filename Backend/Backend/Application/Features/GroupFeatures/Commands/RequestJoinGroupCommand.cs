using Application.DTOs.Firebase;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
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
    public class RequestJoinGroupCommand : IRequest<Response<string>>
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
    public class RequestJoinGroupCommandHandler : IRequestHandler<RequestJoinGroupCommand, Response<string>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetail;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IGroupRepositoryAsync _groupRepository;

        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly INotificationRepositoryAsync _notificationRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IImageRepository _imageRepository;
        private JsonSerializerSettings _settings;

        public RequestJoinGroupCommandHandler(IGroupRepositoryAsync groupRepository,IUserRepositoryAsync userRepository,IGroupMemberDetailRepositoryAsync groupMemberDetail, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, INotificationRepositoryAsync notificationRepository, IFirebaseSerivce firebaseSerivce, IImageRepository imageRepository)
        {
            _groupMemberDetail = groupMemberDetail;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _notificationRepository = notificationRepository;
            _firebaseSerivce = firebaseSerivce;
            _imageRepository = imageRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
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
            var user = await _userRepository.GetUserInfoById(request.UserId);
            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            var data = new JoinRequestData
            {
                UserId = request.UserId,
                FullName = user.FullName,
                GroupId =request.GroupId,
                GroupName = group.GroupName,
                AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(user.Avatar.FileName),
            };

            var messageData = JsonConvert.SerializeObject(data, _settings);


            var admins = await _groupAdminDetailRepository.GetAdminsByGroupId(request.GroupId);
            List<string> sendTokens = new List<string>();
            for (int i = 0; i < admins.Count; i++)
            {
                var tokens = await _firebaseTokenRepository.GetListFirebaseToken(admins[i].AdminId);
                sendTokens.AddRange(tokens);
                await _notificationRepository.AddAsync(new Notification
                {
                    Type = "9",
                    Data = messageData,
                    UserId = admins[i].AdminId,
                    CreateTime = DateTime.UtcNow
                });
            }
            if (sendTokens.Count > 0)
            {

                var responses = await _firebaseSerivce.SendMessage(sendTokens, messageData, NotificationType.JOIN_REQUEST);
                _firebaseTokenRepository.CleanExpiredToken(sendTokens, responses);

            }
            return new Response<string>("Success", null);
        }
    }
}
