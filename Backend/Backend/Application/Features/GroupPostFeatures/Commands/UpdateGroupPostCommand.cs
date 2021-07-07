using Application.DTOs.GroupPost;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupPostFeatures.Commands
{
    public class UpdateGroupPostCommand : UpdateGroupPostRequest, IRequest<Response<GroupPostResponse>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
    public class UpdateGroupPostCommandHandler : IRequestHandler<UpdateGroupPostCommand, Response<GroupPostResponse>>
    {
        private readonly IGroupPostRepositoryAsync _groupPostRepository;
        private readonly IGroupPostImageRelationshipRepositoryAsync _groupPostImageRelationship;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public UpdateGroupPostCommandHandler(IGroupPostRepositoryAsync groupPostRepository, IGroupPostImageRelationshipRepositoryAsync groupPostImageRelationship, IImageRepository imageRepository, IMapper mapper)
        {
            _groupPostRepository = groupPostRepository;
            _groupPostImageRelationship = groupPostImageRelationship;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<Response<GroupPostResponse>> Handle(UpdateGroupPostCommand request, CancellationToken cancellationToken)
        {
            var post = await _groupPostRepository.GetGroupPostForUpdatingById(request.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("PostId not found");
            }
            if (post.PostByAccountId != request.UserId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            post.Content = request.Content;
            post.Visibility = request.Visibility;

            //delete image
            foreach (var i in request.DeletedImages)
            {
                var split = i.Split('/');
                var relate = post.GroupPostImageRelationships.Where(r => r.Image.FileName == split[split.Length - 1]);
                var image = relate.Select(r => r.Image).FirstOrDefault();
                await _imageRepository.DeleteAsync(image);
                post.GroupPostImageRelationships.Remove(relate.FirstOrDefault());
            }

            var result = new GroupPostResponse();
            result.Id = post.Id;
            result.ImageUploads = new List<GroupPostResponse.ImageUpload>();

            for (int i = 0; i < request.ImageNumber; i++)
            {
                string fileName = Guid.NewGuid().ToString();
                var image = new Image { FileName = fileName };
                await _imageRepository.AddAsync(image);

                var relationship = new GroupPostImageRelationship { ImageId = image.Id, PostId = post.Id };
                post.GroupPostImageRelationships.Add(relationship);

                string signUrl = _imageRepository.GenerateV4UploadSignedUrl(fileName);
                result.ImageUploads.Add(new GroupPostResponse.ImageUpload { ImageName = fileName, PresignUrl = signUrl });
            }

            await _groupPostRepository.UpdateAsync(post);

            return new Response<GroupPostResponse>(result);
        }
    }
}
