using Application.DTOs.Address;
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

namespace Application.Features.Events.Commands
{
    public partial class DonateItemForEventCommand : IRequest<Response<PostItemResponse>>
    {
        public int EventId { get; set; }
        public string ItemName { get; set; }
        public AddressDTO ReceiveAddress { get; set; }
        public int CategoryId { get; set; }
        public int DonateAccountId { get; set; }
        public string Description { get; set; }
        public int ImageNumber { get; set; }
    }
    public class DonateItemForEventCommandHandle : IRequestHandler<DonateItemForEventCommand, Response<PostItemResponse>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IDonateEventInformationRepositoryAsync _donateEventInformationRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IItemImageRelationshipRepositoryAsync _itemImageRelationshipRepository;
        private readonly IAddressRepositoryAsync _addressRepository;
        private readonly IMapper _mapper;
        public DonateItemForEventCommandHandle(
            IItemRepositoryAsync itemRepository, IEventRepositoryAsync eventRepository, IDonateEventInformationRepositoryAsync donateEventInformationRepository,
            IImageRepository imageRepository, IItemImageRelationshipRepositoryAsync itemImageRelationshipRepository, 
            IAddressRepositoryAsync addressRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _itemRepository = itemRepository;
            _eventRepository = eventRepository;
            _donateEventInformationRepository = donateEventInformationRepository;
            _itemImageRelationshipRepository = itemImageRelationshipRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }
        public async Task<Response<PostItemResponse>> Handle(DonateItemForEventCommand request, CancellationToken cancellationToken)
        {
            var eventInfo = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventInfo == null) throw new KeyNotFoundException($"Event Not Found.");

            var item = _mapper.Map<Item>(request);
            item.DonateType = (int)EDonateType.DONATE_EVENT;
            item.Status = (int)ItemStatus.NOT_YET;
            item.PostTime = DateTime.Now.ToUniversalTime();
            var address = _mapper.Map<Address>(request.ReceiveAddress);
            await _addressRepository.AddAsync(address);
            item.AddressId = address.Id;
            await _itemRepository.AddAsync(item);

            var donateEventInfo = new DonateEventInformation { EventId = request.EventId, ItemId = item.Id};
            await _donateEventInformationRepository.AddAsync(donateEventInfo);

            var response = new PostItemResponse() { Id = item.Id };
            response.ImageUploads = new List<PostItemResponse.ImageUpload>();
            for (int i = 0; i < request.ImageNumber; i++)
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
