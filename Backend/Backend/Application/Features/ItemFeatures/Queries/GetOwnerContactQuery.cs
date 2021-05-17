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

        public GetOwnerContactHandler(IItemRepositoryAsync itemRepository, IMapper mapper, IAccountService accountServices, IReceiveItemInformationRepositoryAsync receiveRequestRepository)
        {
            _itemRepository = itemRepository;
            _accountServices = accountServices;
            _receiveRequestRepository = receiveRequestRepository;
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
            if (email == null) throw new ApiException("User not found!");
            var reponse = new GetOwnerContactResponse { PhoneNumber = null, Email = email };
            return new Response<GetOwnerContactResponse>(reponse);
        }
    }
}
