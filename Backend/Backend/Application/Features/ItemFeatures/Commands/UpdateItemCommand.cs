using Application.DTOs.Item;
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

namespace Application.Features.ItemFeatures.Commands
{
    public class UpdateItemCommand : UpdateItemDTO, IRequest<Response<PostItemResponse>>
    {
        public int Id { get; set; }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Response<PostItemResponse>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public UpdateItemCommandHandler(IItemRepositoryAsync itemRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<PostItemResponse>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItemDetailForUpdatingById(request.Id);
            if (item == null)
            {
                throw new KeyNotFoundException("ItemId not found");
            }

            if (request.ReceiveAddress != null)
            {
                item.Address = _mapper.Map<Address>(request.ReceiveAddress);
            }
            item.CategoryId = request.CategoryId;
            item.Description = request.Description;
            item.ItemName = request.ItemName;

            //delete image
            foreach (var i in request.DeletedImages)
            {
                var split = i.Split('/');
                var relate = item.ItemImageRelationships.Where(r => r.Image.FileName == split[split.Length - 1]);
                var image = relate.Select(r => r.Image).FirstOrDefault();
                await _imageRepository.DeleteAsync(image);
                item.ItemImageRelationships.Remove(relate.FirstOrDefault());
            }

            var result = new PostItemResponse();
            result.Id = item.Id;
            result.ImageUploads = new List<PostItemResponse.ImageUpload>();

            for (int i = 0; i < request.ImageNumber; i++)
            {
                string fileName = Guid.NewGuid().ToString();
                var image = new Image { FileName = fileName };
                await _imageRepository.AddAsync(image);

                var relationship = new ItemImageRelationship { ImageId = image.Id, ItemId = item.Id };
                item.ItemImageRelationships.Add(relationship);

                string signUrl = _imageRepository.GenerateV4UploadSignedUrl(fileName);
                result.ImageUploads.Add(new PostItemResponse.ImageUpload { ImageName = fileName, PresignUrl = signUrl });
            }

            await _itemRepository.UpdateAsync(item);

            return new Response<PostItemResponse>(result);
        }
    }
}
