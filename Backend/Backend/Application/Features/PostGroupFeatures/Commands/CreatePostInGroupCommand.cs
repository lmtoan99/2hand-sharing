using Application.DTOs.GroupPost;
using Application.Enums;
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

namespace Application.Features.PostGroupFeatures.Commands
{
    public partial class CreatePostInGroupCommand : IRequest<Response<GroupPostResponse>>
    {
        public string Content { get; set; }
        public int GroupId { get; set; }
        public int PostByAccountId { get; set; }
        public int Visibility { get; set; }
        public int ImageNumber { get; set; }
    }
    public class CreatePostInGroupCommandHandle : IRequestHandler<CreatePostInGroupCommand, Response<GroupPostResponse>>
    {
        private readonly IGroupPostRepositoryAsync _groupPostRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IGroupPostImageRelationshipRepositoryAsync _groupPostImageRelationshipRepository;
        private readonly IMapper _mapper;
        public CreatePostInGroupCommandHandle(IGroupPostRepositoryAsync groupPostRepository, IImageRepository imageRepository, IGroupPostImageRelationshipRepositoryAsync groupPostImageRelationshipRepository, IMapper mapper)
        {
            _groupPostRepository = groupPostRepository;
            _imageRepository = imageRepository;
            _groupPostImageRelationshipRepository = groupPostImageRelationshipRepository;
            _mapper = mapper;
        }
        public async Task<Response<GroupPostResponse>> Handle(CreatePostInGroupCommand request, CancellationToken cancellationToken)
        {
            var groupPost = _mapper.Map<GroupPost>(request);
            groupPost.Visibility = request.Visibility;
            groupPost.PostTime = DateTime.Now.ToUniversalTime();
          
            await _groupPostRepository.AddAsync(groupPost);

            var response = new GroupPostResponse() { Id = groupPost.Id };
            response.ImageUploads = new List<GroupPostResponse.ImageUpload>();
            for (int i = 0; i < request.ImageNumber; i++)
            {
                string fileName = Guid.NewGuid().ToString();
                var image = new Image { FileName = fileName };
                await _imageRepository.AddAsync(image);

                var relationship = new GroupPostImageRelationship { ImageId = image.Id, PostId = groupPost.Id };
                await _groupPostImageRelationshipRepository.AddAsync(relationship);

                string signUrl = _imageRepository.GenerateV4UploadSignedUrl(fileName);
                response.ImageUploads.Add(new GroupPostResponse.ImageUpload { ImageName = fileName, PresignUrl = signUrl });
            }

            return new Response<GroupPostResponse>(response);
        }
    }
}
