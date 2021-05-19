using Application.DTOs.Account;
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

namespace Application.Features.ReceiveItemInformationFeatures.Queries
{
    public class GetReceiverInfoByIdQuery : IRequest<Response<UserInfoDTO>>
    {
        public int ReceiverId { get; set; }
    }
    public class GetReceiverInfoByIdHandler : IRequestHandler<GetReceiverInfoByIdQuery, Response<UserInfoDTO>>
    {
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;

        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public GetReceiverInfoByIdHandler(IReceiveItemInformationRepositoryAsync receiveItemInformationRepository, IMapper mapper, IAccountService accountService)
        {
            _receiveItemInformationRepository = receiveItemInformationRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Response<UserInfoDTO>> Handle(GetReceiverInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var receiveItemInformation = await _receiveItemInformationRepository.GetReceiveRequestByWithReceiverInfoById(request.ReceiverId);
            if (receiveItemInformation == null) throw new ApiException("Request not found");
            var user = receiveItemInformation.Receiver;

            var response = new Response<UserInfoDTO>(_mapper.Map<UserInfoDTO>(user));
            response.Data.Email = await _accountService.GetEmailById(user.AccountId);
            return response;
        }
    }
}
