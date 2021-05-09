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
    public partial class UpdateUserAvatarCommand:IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class UpdateUserAvatarCommandHandler:IRequestHandler<UpdateUserAvatarCommand,Response<int>>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public UpdateUserAvatarCommandHandler(IUserRepositoryAsync userRepository,IImageRepository imageRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            return new Response<int>(user.Id);
        }
    }
}
