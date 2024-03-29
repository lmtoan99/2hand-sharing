﻿using Application.DTOs.Item;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ItemFeatures.Queries
{
    public class GetListReceiveRequestQuery : IRequest<Response<IEnumerable<ReceiveRequestDonorViewModel>>>
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
    }
    public class GetListReceiveRequestQueryHandler : IRequestHandler<GetListReceiveRequestQuery, Response<IEnumerable<ReceiveRequestDonorViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetListReceiveRequestQueryHandler(IItemRepositoryAsync itemRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _itemRepository = itemRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<ReceiveRequestDonorViewModel>>> Handle(GetListReceiveRequestQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItemWithReceiveRequestByIdAsync(request.ItemId);
            if (item == null) throw new KeyNotFoundException("ItemId not found");
            if (item.DonateAccountId != request.UserId) throw new UnauthorizedAccessException();
            if (item.Status == (int)ItemStatus.SUCCESS) throw new ApiException("Item is not able to receive");
            var response = _mapper.Map<IEnumerable<ReceiveRequestDonorViewModel>>(item.ReceiveItemInformations);
            foreach(ReceiveRequestDonorViewModel i in response)
            {
                i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
            }
            return new Response<IEnumerable<ReceiveRequestDonorViewModel>>(response);
        }
    }
}
