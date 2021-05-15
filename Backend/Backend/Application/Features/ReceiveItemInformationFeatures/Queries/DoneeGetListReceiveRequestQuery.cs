using Application.DTOs.Item;
using Application.DTOs.ReceiveRequest;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ReceiveItemInformationFeatures.Queries
{
    public class DoneeGetListReceiveRequestQuery : IRequest<PagedResponse<IReadOnlyCollection<GetAllItemViewModel>>>
    {
        public int UserId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
    public class DoneeGetListReceiveRequestQueryHandler : IRequestHandler<DoneeGetListReceiveRequestQuery, PagedResponse<IReadOnlyCollection<GetAllItemViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public DoneeGetListReceiveRequestQueryHandler(IItemRepositoryAsync itemRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyCollection<GetAllItemViewModel>>> Handle(DoneeGetListReceiveRequestQuery request, CancellationToken cancellationToken)
        {
            var list = await _itemRepository.GetAllItemHaveRequestWithReceiverId(request.UserId, request.PageNumber, request.PageSize);
            var result = _mapper.Map<List<GetAllItemViewModel>>(list);

            result.ForEach(i => {
                i.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ImageUrl);
            });
            return new PagedResponse<IReadOnlyCollection<GetAllItemViewModel>>(result, request.PageNumber, request.PageSize);
        }
    }
}
