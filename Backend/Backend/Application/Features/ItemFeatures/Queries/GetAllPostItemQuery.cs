﻿using Application.DTOs.Address;
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

namespace Application.Features.ItemFeatures.Queries
{
    public class GetAllPostItemQuery : IRequest<PagedResponse<IEnumerable<GetAllItemViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
        public int CategoryId { get; set; }
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
            IReadOnlyCollection<Item> item;
            if (request.Query != null)
            {
                if (request.CategoryId != 0)
                {
                    item = await _itemRepository.SearchPostItemsWithCategoryIdAsync(request.Query, request.CategoryId, request.PageNumber, request.PageSize);
                }
                else
                {
                    item = await _itemRepository.SearchPostItemsAsync(request.Query, request.PageNumber, request.PageSize);
                }
            }
            else if (request.CategoryId != 0)
            {
                item = await _itemRepository.GetAllPostItemsByCategoryIdAsync(request.PageNumber, request.PageSize, request.CategoryId);
            }
            else
            {
                item = await _itemRepository.GetAllPostItemsAsync(validFilter.PageNumber, validFilter.PageSize);
            }
            
            List<GetAllItemViewModel> itemViewModel = _mapper.Map<List<GetAllItemViewModel>>(item);
            itemViewModel.ForEach(i =>
            {
                i.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ImageUrl);
                i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
            });

            return new PagedResponse<IEnumerable<GetAllItemViewModel>>(itemViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
