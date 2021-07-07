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

    public class CancelJoinRequestCommand : IRequest<Response<string>>
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
    public class CancelJoinRequestCommandHandler : IRequestHandler<CancelJoinRequestCommand, Response<string>>
    {
        public IGroupMemberDetailRepositoryAsync _groupMemberDetail;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly INotificationRepositoryAsync _notificationRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IImageRepository _imageRepository;
        private JsonSerializerSettings _settings;

        public CancelJoinRequestCommandHandler(IGroupMemberDetailRepositoryAsync groupMemberDetail, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, INotificationRepositoryAsync notificationRepository, IFirebaseSerivce firebaseSerivce, IImageRepository imageRepository)
        {
            _groupMemberDetail = groupMemberDetail;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _notificationRepository = notificationRepository;
            _firebaseSerivce = firebaseSerivce;
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
