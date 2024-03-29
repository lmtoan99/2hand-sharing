﻿using Application.DTOs.Address;
using Application.DTOs.Item;
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

namespace Application.Features.ItemFeatures.Commands
{
    public partial class PostItemCommand : IRequest<Response<PostItemResponse>>
    {
        public string ItemName { get; set; }
        public AddressDTO ReceiveAddress { get; set; }
        public int CategoryId { get; set; }
        public int DonateAccountId { get; set; }
        public string Description { get; set; }
        public int ImageNumber { get; set; }
    }
    public class PostItemCommandHandle : IRequestHandler<PostItemCommand, Response<PostItemResponse>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IItemImageRelationshipRepositoryAsync _itemImageRelationshipRepository;
        private readonly IAddressRepositoryAsync _addressRepository;
        private readonly IMapper _mapper;
        public PostItemCommandHandle(IItemRepositoryAsync itemRepository, IImageRepository imageRepository, IItemImageRelationshipRepositoryAsync itemImageRelationshipRepository, IAddressRepositoryAsync addressRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _itemRepository = itemRepository;
            _itemImageRelationshipRepository = itemImageRelationshipRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }
        public async Task<Response<PostItemResponse>> Handle(PostItemCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Item>(request);
            item.DonateType = (int)EDonateType.DONATE_POST;
            item.Status = (int)ItemStatus.NOT_YET;
            item.PostTime = DateTime.Now.ToUniversalTime();
            var address = _mapper.Map<Address>(request.ReceiveAddress);
            await _addressRepository.AddAsync(address);
            item.AddressId = address.Id;
            await _itemRepository.AddAsync(item);

            var response = new PostItemResponse() { Id = item.Id };
            response.ImageUploads = new List<PostItemResponse.ImageUpload>();
            for (int i = 0; i < request.ImageNumber;i++)
            {
                string fileName = Guid.NewGuid().ToString();
                var image = new Image { FileName = fileName };
                await _imageRepository.AddAsync(image);

                var relationship = new ItemImageRelationship { ImageId = image.Id, ItemId = item.Id };
                await _itemImageRelationshipRepository.AddAsync(relationship);

                string signUrl = _imageRepository.GenerateV4UploadSignedUrl(fileName);
                response.ImageUploads.Add(new PostItemResponse.ImageUpload { ImageName = fileName, PresignUrl = signUrl });
            }

            return new Response<PostItemResponse>(response);
        }
    }
}
