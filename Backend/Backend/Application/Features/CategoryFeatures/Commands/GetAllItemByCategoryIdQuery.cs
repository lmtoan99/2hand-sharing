using Application.DTOs.Address;
using Application.Features.ItemFeatures.Queries;
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

namespace Application.Features.CategoryFeatures.Commands
{
    public class GetAllItemByCategoryIdQuery : IRequest<PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int CategoryId { get; set; }
    }
    public class GetAllItemByCategoryIdQueryHandler : IRequestHandler<GetAllItemByCategoryIdQuery, PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetAllItemByCategoryIdQueryHandler(IItemRepositoryAsync itemRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllItemViewModel>>> Handle(GetAllItemByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllItemsParameter>(request);
            var item = await _itemRepository.GetAllPostItemsByCategoryIdAsync(validFilter.PageNumber, validFilter.PageSize,request.CategoryId);
            var itemViewModel = _mapper.Map<List<GetAllItemViewModel>>(item);
           
            var address = item.Select(item => item.Address).ToArray();
            for(int i=0;i<itemViewModel.Count;i++)
            {
                itemViewModel[i].ReceiveAddress = _mapper.Map<AddressDTO>(address[i]);
            }

            itemViewModel.ForEach(item =>
            {
                item.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(item.ImageUrl);
            });

            return new PagedResponse<IEnumerable<GetAllItemViewModel>>(itemViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }

}
