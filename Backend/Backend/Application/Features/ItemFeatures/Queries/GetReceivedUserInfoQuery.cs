using Application.DTOs.Account;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
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
    public class GetReceivedUserInfoQuery : IRequest<Response<UserInfoDTO>>
    {
        public int itemId;
    }
    public class GetReceivedUserInfoQueryHandler : IRequestHandler<GetReceivedUserInfoQuery, Response<UserInfoDTO>>
    {
        IReceiveItemInformationRepositoryAsync _receiveRequestRepository;
        IItemRepositoryAsync _itemRepository;
        IUserRepositoryAsync _userRepository;
        IImageRepository _imageRepository;

        public GetReceivedUserInfoQueryHandler(IReceiveItemInformationRepositoryAsync receiveRequestRepository, IMapper mapper, IItemRepositoryAsync itemRepository, IUserRepositoryAsync userRepository)
        {
            _userRepository = userRepository;
            _receiveRequestRepository = receiveRequestRepository;
            _itemRepository = itemRepository;
        }

        public async Task<Response<UserInfoDTO>> Handle(GetReceivedUserInfoQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(request.itemId);
            if (item == null) throw new ApiException("Item not existed");
            if (item.Status != (int)ItemStatus.SUCCESS) throw new ApiException("Item isn't given for anyone");

            var requests = await _receiveRequestRepository.GetAllByItemId(request.itemId);
            for (int i = 0; i < requests.Count; i++)
            {
                if(requests[i].ReceiveStatus ==(int) ReceiveItemInformationStatus.RECEIVING)
                {
                   string fullName = await _userRepository.GetUserFullnameById(requests[i].ReceiverId);
                    UserInfoDTO userInfoDTO = new UserInfoDTO { FullName = fullName, Id = requests[i].ReceiverId };
                    return new Response<UserInfoDTO>(userInfoDTO);
                }
            }
            return null;
        }
    }
}
