using Application.DTOs.Item;
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
    public class GetListItemDonateByDonorIdQuery : IRequest<Response<IEnumerable<GetListMyItemDonateViewModel>>>
    {
        public int userId { get; set; }
    }
    public class GetListItemDonateByDonorIdQueryHandler : IRequestHandler<GetListItemDonateByDonorIdQuery, Response<IEnumerable<GetListMyItemDonateViewModel>>>
    {
        private readonly IItemRepositoryAsync _itemRepositoryAsync;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetListItemDonateByDonorIdQueryHandler(IItemRepositoryAsync itemRepositoryAsync, IImageRepository imageRepository, IMapper mapper)
        {
            _itemRepositoryAsync = itemRepositoryAsync;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GetListMyItemDonateViewModel>>> Handle(GetListItemDonateByDonorIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _itemRepositoryAsync.GetItemByDonateAccountId(request.userId);
            var res = _mapper.Map<List<GetListMyItemDonateViewModel>>(result);
            res.ForEach(i =>
            {
                i.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ImageUrl);
            });
            return new Response<IEnumerable<GetListMyItemDonateViewModel>>(res);
        }
    }
}
