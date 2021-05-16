using Application.DTOs.Account;
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

namespace Application.Features.AccountsFeature.Queries
{
    public class GetUserInfoQuery : IRequest<Response<UserInfoDTO>>
    {
        public int UserId { get; set; }
    }
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, Response<UserInfoDTO>>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IMapper _mapper;
        public GetUserInfoQueryHandler(IUserRepositoryAsync userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<Response<UserInfoDTO>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserInfoById(request.UserId);
            if (user == null) throw new ApiException("User id not found");
            return new Response<UserInfoDTO>(_mapper.Map<UserInfoDTO>(user));
        }
    }
}
