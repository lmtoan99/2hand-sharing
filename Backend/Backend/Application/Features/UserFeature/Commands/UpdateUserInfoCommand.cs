using Application.DTOs.Account;
using Application.DTOs.Address;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeature.Commands
{
    public class UpdateUserInfoCommand : IRequest<Response<UserInfoDTO>>
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO Address { get; set; }
    }
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, Response<UserInfoDTO>>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IAddressRepositoryAsync _addressRepository;
        private readonly IMapper _mapper;
        public UpdateUserInfoCommandHandler(IUserRepositoryAsync userRepository, IAddressRepositoryAsync addressRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }
        public async Task<Response<UserInfoDTO>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new UnauthorizedAccessException("Bad AccessToken");
            if (request.FullName != null) user.FullName = request.FullName;
            if (request.Dob.Ticks > 0) user.Dob = request.Dob;
            if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
            if (request.Address != null)
            {
                var address = await _addressRepository.AddAsync(_mapper.Map<Address>(request.Address));
                user.AddressId = address.Id;
            }
            await _userRepository.UpdateAsync(user);
            return new Response<UserInfoDTO>(_mapper.Map<UserInfoDTO>(user));
        }
    }
}
