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
    public class AcceptInvitationCommand : IRequest<Response<string>>
    {
        public int UserId;
        public int GroupId;
    }
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Response<string>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly INotificationRepositoryAsync _notificationRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IImageRepository _imageRepository;
        private JsonSerializerSettings _settings;

        public AcceptInvitationCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepository,IGroupMemberDetailRepositoryAsync groupMemberDetailRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, INotificationRepositoryAsync notificationRepository, IFirebaseSerivce firebaseSerivce, IImageRepository imageRepository)
        {
            _groupMemberDetailRepository = groupMemberDetailRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _notificationRepository = notificationRepository;
            _firebaseSerivce = firebaseSerivce;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _imageRepository = imageRepository;
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

        public async Task<Response<string>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            var member = await _groupMemberDetailRepository.GetInvitation(request.GroupId, request.UserId);
            if (member == null)
            {
                throw new ApiException("Invitation not exists");
            }

            var data = new AcceptInvitationData
            {
                UserId = member.MemberId,
                FullName = member.Member.FullName,
                GroupId = member.GroupId,
                GroupName = member.Group.GroupName,
                AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(member.Group.Avatar.FileName),
            };

            member.JoinStatus = (int)MemberJoinStatus.ACCEPTED;
            await _groupMemberDetailRepository.UpdateAsync(member);
            var messageData = JsonConvert.SerializeObject(data, _settings);


            var admins = await _groupAdminDetailRepository.GetAdminsByGroupId(request.GroupId);
            List<string> sendTokens = new List<string>();
            for (int i = 0; i < admins.Count; i++)
            {
                var tokens = await _firebaseTokenRepository.GetListFirebaseToken(admins[i].AdminId);
                sendTokens.AddRange(tokens);
                await _notificationRepository.AddAsync(new Notification
                {
                    Type = "8",
                    Data = messageData,
                    UserId = admins[i].AdminId,
                    CreateTime = DateTime.UtcNow
                });
            }
            if (sendTokens.Count > 0)
            {

                var responses = await _firebaseSerivce.SendMessage(sendTokens, messageData, NotificationType.ACCEPT_INVITATION);
                _firebaseTokenRepository.CleanExpiredToken(sendTokens, responses);

            }
            return new Response<string>(null);
        }
    }
}
