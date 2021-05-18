using Application.DTOs.Item;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ItemFeatures.Queries
{
    public class GetOwnerContactQuery : IRequest<Response<GetOwnerContactResponse>>
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
    }
    public class GetOwnerContactHandler : IRequestHandler<GetOwnerContactQuery, Response<GetOwnerContactResponse>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IReceiveItemInformationRepositoryAsync _receiveRequestRepository;
        private readonly IAccountService _accountServices;
        private readonly IUserRepositoryAsync _userRepository;

        public GetOwnerContactHandler(IItemRepositoryAsync itemRepository, IReceiveItemInformationRepositoryAsync receiveRequestRepository, IAccountService accountServices, IUserRepositoryAsync userRepository)
        {
            _itemRepository = itemRepository;
            _receiveRequestRepository = receiveRequestRepository;
            _accountServices = accountServices;
            _userRepository = userRepository;
        }


        public async Task<Response<GetOwnerContactResponse>> Handle(GetOwnerContactQuery request, CancellationToken cancellationToken)
        {
            var receiveRequest = await _receiveRequestRepository.GetReceiveRequestByItemIdAndUserId(request.ItemId, request.UserId);
            var item = await _itemRepository.GetItemContactByIdAsync(request.ItemId);
            if (item == null) throw new ApiException("Item not found!");
            if (receiveRequest == null || receiveRequest.ReceiveStatus != (int) ReceiveItemInformationStatus.RECEIVING)
            {
                throw new ApiException("Permission denied: User's request isn't accepted!");
            }

            var email = await _accountServices.GetEmailById(item.DonateAccount.AccountId);
            var user = await _userRepository.GetByIdAsync(item.DonateAccount.Id);
            if (email == null || user == null) throw new ApiException("User not found!");
            var reponse = new GetOwnerContactResponse { PhoneNumber = user.PhoneNumber, Email = email };
            return new Response<GetOwnerContactResponse>(reponse);
        }
    }
}
