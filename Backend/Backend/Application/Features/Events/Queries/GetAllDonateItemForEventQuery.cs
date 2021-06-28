using Application.DTOs.Item;
using Application.Features.ItemFeatures.Queries;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Events.Queries
{
    public class GetAllDonateItemForEventQuery : IRequest<PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        public int EventId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllDonateItemForEventQueryHandler : IRequestHandler<GetAllDonateItemForEventQuery, PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IDonateEventInformationRepositoryAsync _donateEventInformationRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetAllDonateItemForEventQueryHandler(IItemRepositoryAsync itemRepository, IDonateEventInformationRepositoryAsync donateEventInformationRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _donateEventInformationRepository = donateEventInformationRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllItemViewModel>>> Handle(GetAllDonateItemForEventQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllItemsParameter>(request);
            var itemDonateEvent = await _itemRepository.GetAllItemDonateForEventAsync(validFilter.PageNumber, validFilter.PageSize, request.EventId);
            List<GetAllItemViewModel> itemViewModel = _mapper.Map<List<GetAllItemViewModel>>(itemDonateEvent);
            itemViewModel.ForEach(i =>
            {
                i.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ImageUrl);
                i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
            });

            return new PagedResponse<IEnumerable<GetAllItemViewModel>>(itemViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
