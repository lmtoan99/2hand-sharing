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

namespace Application.Features.ItemFeatures.Queries
{
    public class GetItemByIdQuery : IRequest<Response<GetAllItemViewModel>>
    {
        public int Id { get; set; }
        
        //private readonly IImageRepository _imageRepository;
        
        public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Response<GetAllItemViewModel>>
        {
            private readonly IItemRepositoryAsync _itemRepository;
            private readonly IMapper _mapper;
            private readonly IImageRepository _imageRepository;
            public GetItemByIdQueryHandler(IItemRepositoryAsync itemRepository, IMapper mapper,IImageRepository imageRepository)
            {
                _itemRepository = itemRepository;
                _mapper = mapper;
                _imageRepository = imageRepository;
            }
            public async Task<Response<GetAllItemViewModel>> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
            {
                var item = await _itemRepository.GetByIdAsync(query.Id);         
                var itemViewModel = _mapper.Map<GetAllItemViewModel>(item);
                itemViewModel.ImageUrl= _imageRepository.GenerateV4SignedReadUrl(itemViewModel.ImageUrl);
                if (item == null) throw new ApiException($"Item Not Found.");
                return new Response<GetAllItemViewModel>(itemViewModel);
            }
        }
    }
}
