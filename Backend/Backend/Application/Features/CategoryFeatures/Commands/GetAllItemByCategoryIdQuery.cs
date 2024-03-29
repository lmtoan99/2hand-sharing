﻿using Application.DTOs.Address;
using Application.DTOs.Item;
using Application.Features.ItemFeatures.Queries;
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

namespace Application.Features.CategoryFeatures.Commands
{
    public class GetAllItemByCategoryIdQuery : IRequest<PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
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
            IReadOnlyCollection<Item> item;
            if (request.Query != null)
            {
                item = await _itemRepository.SearchPostItemsWithCategoryIdAsync(request.Query, request.CategoryId, request.PageNumber, request.PageSize);
            } else
            {
                item = await _itemRepository.GetAllPostItemsByCategoryIdAsync(validFilter.PageNumber, validFilter.PageSize, request.CategoryId);
            }
            var itemViewModel = _mapper.Map<List<GetAllItemViewModel>>(item);

            itemViewModel.ForEach(item =>
            {
                item.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(item.ImageUrl);
                item.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(item.AvatarUrl);
            });

            return new PagedResponse<IEnumerable<GetAllItemViewModel>>(itemViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }

}
