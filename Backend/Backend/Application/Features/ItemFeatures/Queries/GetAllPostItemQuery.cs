using Application.DTOs.Address;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ItemFeatures.Queries
{
    public class GetAllPostItemQuery : IRequest<PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllItemsQueryHandler : IRequestHandler<GetAllPostItemQuery, PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetAllItemsQueryHandler(IItemRepositoryAsync itemRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllItemViewModel>>> Handle(GetAllPostItemQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllItemsParameter>(request);
            var item = await _itemRepository.GetAllPostItemsAsync(validFilter.PageNumber, validFilter.PageSize);
            List<GetAllItemViewModel> itemViewModel = new List<GetAllItemViewModel>();
            for (int i = 0; i < item.Count; i++)
            {
                GetAllItemViewModel viewItem = _mapper.Map<GetAllItemViewModel>(item.ElementAt(i));
                viewItem.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(viewItem.ImageUrl);
                viewItem.ReceiveAddress = _mapper.Map<AddressDTO>(item.ElementAt(i).Address);
                itemViewModel.Add(viewItem);
            }

            return new PagedResponse<IEnumerable<GetAllItemViewModel>>(itemViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
