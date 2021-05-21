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
    public class GetUserInfoByIdQuery : IRequest<Response<UserInfoDTO>>
    {
        public int UserId { get; set; }
    }
    public class GetReceiverInfoByIdHandler : IRequestHandler<GetUserInfoByIdQuery, Response<UserInfoDTO>>
    {

        private readonly IUserRepositoryAsync _userRepository;

        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public GetReceiverInfoByIdHandler(IUserRepositoryAsync userRepository, IMapper mapper, IAccountService accountService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Response<UserInfoDTO>> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new ApiException("User not found");

            var response = new Response<UserInfoDTO>(_mapper.Map<UserInfoDTO>(user));
            response.Data.Email = await _accountService.GetEmailById(user.AccountId);
            return response;
        }
    }
}
