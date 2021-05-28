using Application.DTOs.Firebase;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ItemFeatures.Commands
{
    public partial class CreateReceiveRequestCommand : IRequest<Response<int>>
    {
        public int ItemId { get; set; }
        public string ReceiveReason { get; set; }
        public int ReceiverId { get; set; }
    }
    public class CreateReceiveRequestCommandHandle : IRequestHandler<CreateReceiveRequestCommand, Response<int>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
        private readonly IFirebaseSerivce _firebaseSerivce;
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        private readonly IUserRepositoryAsync _userRepository;

        public CreateReceiveRequestCommandHandle(IFirebaseSerivce firebaseSerivce, IItemRepositoryAsync itemRepository,
            IReceiveItemInformationRepositoryAsync receiveItemInformationRepository, IFirebaseTokenRepositoryAsync firebaseTokenRepository, IUserRepositoryAsync userRepository)
        {
            _itemRepository = itemRepository;
            _receiveItemInformationRepository = receiveItemInformationRepository;
            _firebaseTokenRepository = firebaseTokenRepository;
            _userRepository = userRepository;
            _firebaseSerivce = firebaseSerivce;
        }
        public async Task<Response<int>> Handle(CreateReceiveRequestCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(request.ItemId);
            if (item == null) throw new KeyNotFoundException("ItemId not found");
            if (item.Status == (int)ItemStatus.SUCCESS) throw new ApiException("Item is not able to create receive request");
            var checkCreated = await _receiveItemInformationRepository.GetByConditionAsync(i => i.ItemId == request.ItemId && i.ReceiverId == request.ReceiverId);
            if (checkCreated.Count > 0) throw new ApiException("User created a receive request on this item");
            if (request.ReceiverId == item.DonateAccountId) throw new ApiException("User can not create request on their item");
            var receiverName = await _userRepository.GetUserFullnameById(request.ReceiverId);
            var newInfo = new Domain.Entities.ReceiveItemInformation
            {
                ItemId = request.ItemId,
                ReceiverId = request.ReceiverId,
                ReceiveReason = request.ReceiveReason,
                ReceiveStatus = (int)ReceiveItemInformationStatus.PENDING,
                CreateDate = DateTime.UtcNow
            };
            ReceiveItemInformation receiveItemInformation = await _receiveItemInformationRepository.AddAsync(newInfo);
            var tokens = await _firebaseTokenRepository.GetListFirebaseToken(item.DonateAccountId);
            if (tokens.Count > 0)
            {
                ReceiveRequestNotificationData data = new ReceiveRequestNotificationData
                {
                    Id = receiveItemInformation.Id,
                    ReceiverId = request.ReceiverId,
                    ReceiverName = receiverName,
                    ItemId = request.ItemId,
                    ItemName = item.ItemName,
                    ReceiveReason = request.ReceiveReason
                };
                var responses = await _firebaseSerivce.SendReceiveRequestNotification(tokens, data);
                _firebaseTokenRepository.CleanExpiredToken(tokens, responses);

            }
            return new Response<int>(newInfo.Id);
        }
    }
}
