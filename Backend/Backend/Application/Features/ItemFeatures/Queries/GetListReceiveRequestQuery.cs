using Application.DTOs.Item;
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
    public class GetListReceiveRequestQuery : IRequest<Response<IEnumerable<ReceiveRequestViewModel>>>
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
    }
    public class GetListReceiveRequestQueryHandler : IRequestHandler<GetListReceiveRequestQuery, Response<IEnumerable<ReceiveRequestViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
        private readonly IMapper _mapper;
        public GetListReceiveRequestQueryHandler(IItemRepositoryAsync itemRepository, IReceiveItemInformationRepositoryAsync receiveItemInformationRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _receiveItemInformationRepository = receiveItemInformationRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<ReceiveRequestViewModel>>> Handle(GetListReceiveRequestQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItemWithReceiveRequestByIdAsync(request.ItemId);
            if (item == null) throw new KeyNotFoundException("ItemId not found");
            if (item.DonateAccountId != request.UserId) throw new UnauthorizedAccessException();
            if (item.Status != (int)ItemStatus.NOT_YET) throw new ApiException("Item is not able to receive");
            var response = _mapper.Map<IEnumerable<ReceiveRequestViewModel>>(item.ReceiveItemInformations);
            return new Response<IEnumerable<ReceiveRequestViewModel>>(response);
        }
    }
}
