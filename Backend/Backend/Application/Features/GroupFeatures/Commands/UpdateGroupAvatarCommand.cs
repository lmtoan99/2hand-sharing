using Application.DTOs.Group;
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


namespace Application.Features.GroupFeatures.Commands
{
    public partial class UpdateGroupAvatarCommand : IRequest<Response<UpdateAvatarResponse>>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }

    public class UpdateGroupAvatarCommandHandler : IRequestHandler<UpdateGroupAvatarCommand, Response<UpdateAvatarResponse>>
    {
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IGroupRepositoryAsync _groupRepository;
        public UpdateGroupAvatarCommandHandler(IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IImageRepository imageRepository, IGroupRepositoryAsync groupRepository)
        {
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _imageRepository = imageRepository;
            _groupRepository = groupRepository;
        }
        public async Task<Response<UpdateAvatarResponse>> Handle(UpdateGroupAvatarCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            string fileName = Guid.NewGuid().ToString();
            var image = new Image { FileName = fileName };
            string signUrl = _imageRepository.GenerateV4UploadSignedUrl(fileName);
            var response = new UpdateAvatarResponse();
            response.ImageUploads = new UpdateAvatarResponse.ImageUpload { ImageName = fileName, PresignUrl = signUrl };
            var groupAdminInfo = await _groupAdminDetailRepository.GetInfoGroupAdminDetail(request.GroupId, request.UserId);
            if (groupAdminInfo == null)
            {
                throw new ApiException("You are not admin of this group.");
            }
            if (group.AvatarId == null)
            {
                await _imageRepository.AddAsync(image);
                group.AvatarId = image.Id;
                response.Id = image.Id;
                await _groupRepository.UpdateAsync(group);
            }
            else
            {
                var avatar = await _imageRepository.GetByIdAsync(group.AvatarId.GetValueOrDefault());
                avatar.FileName = fileName;
                await _imageRepository.UpdateAsync(avatar);
                group.AvatarId = avatar.Id;
                response.Id = avatar.Id;
                await _groupRepository.UpdateAsync(group);
            }

            return new Response<UpdateAvatarResponse>(response);
        }
    }
}
