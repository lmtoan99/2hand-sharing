using Application.DTOs.Address;
using Application.DTOs.Item;
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
    public class GetItemByIdQuery : IRequest<Response<GetItemByIdViewModel>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Response<GetItemByIdViewModel>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
        public GetItemByIdQueryHandler(IItemRepositoryAsync itemRepository, IMapper mapper, IImageRepository imageRepository, IReceiveItemInformationRepositoryAsync receiveItemInformationRepository)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
            _receiveItemInformationRepository = receiveItemInformationRepository;
        }
        public async Task<Response<GetItemByIdViewModel>> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItemDetailByIdAsync(query.Id);
            if (item == null) throw new KeyNotFoundException($"Item Not Found.");

            var itemViewModel = _mapper.Map<GetItemByIdViewModel>(item);
            itemViewModel.ReceiveAddress = _mapper.Map<AddressDTO>(item.Address);

            for (int i = 0; i < itemViewModel.ImageUrl.Count; i++)
            {
                itemViewModel.ImageUrl[i] = _imageRepository.GenerateV4SignedReadUrl(itemViewModel.ImageUrl[i]);
            }

            var req = await _receiveItemInformationRepository.GetByConditionAsync(r => r.ItemId == item.Id && r.ReceiverId == query.UserId);

            if (req.Count > 0)
                itemViewModel.UserRequestId = req[0].Id;

            return new Response<GetItemByIdViewModel>(itemViewModel);
        }
    }
}
