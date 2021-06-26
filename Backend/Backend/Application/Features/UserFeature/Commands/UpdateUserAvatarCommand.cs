using Application.DTOs.Account;
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
    public partial class UpdateUserAvatarCommand:IRequest<Response<UpdateAvatarResponse>>
    {
        public int UserId { get; set; }
    }

    public class UpdateUserAvatarCommandHandler:IRequestHandler<UpdateUserAvatarCommand,Response<UpdateAvatarResponse>>
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
        public async Task<Response<UpdateAvatarResponse>> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            string fileName = Guid.NewGuid().ToString();
            var image = new Image { FileName = fileName };
            string signUrl = _imageRepository.GenerateV4UploadSignedUrl(fileName);
            var response = new UpdateAvatarResponse();
            response.ImageUploads = new UpdateAvatarResponse.ImageUpload { ImageName = fileName, PresignUrl = signUrl };

            if (user.AvatarId == null)
            {   
                await _imageRepository.AddAsync(image);
                user.AvatarId = image.Id;
                await _userRepository.UpdateAsync(user);
            }
            else
            {
                var userAvatar = await _imageRepository.GetByIdAsync(user.AvatarId.GetValueOrDefault());
                userAvatar.FileName = fileName;
                await _imageRepository.UpdateAsync(userAvatar);
                user.AvatarId = userAvatar.Id;
                await _userRepository.UpdateAsync(user);
            }

            return new Response<UpdateAvatarResponse>(response);
        }
    }
}
