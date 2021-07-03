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
    public class GetListItemDonateByDonorIdQuery : IRequest<Response<IEnumerable<GetMyDonatedItemsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int userId { get; set; }
    }
    public class GetListItemDonateByDonorIdQueryHandler : IRequestHandler<GetListItemDonateByDonorIdQuery, Response<IEnumerable<GetMyDonatedItemsViewModel>>>
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
        public async Task<Response<IEnumerable<GetMyDonatedItemsViewModel>>> Handle(GetListItemDonateByDonorIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _itemRepositoryAsync.GetItemByDonateAccountId(request.userId, request.PageNumber, request.PageSize);
            var res = _mapper.Map<List<GetMyDonatedItemsViewModel>>(result);
            res.ForEach(i =>
            {
                i.ImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ImageUrl);
                i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
            });
            return new Response<IEnumerable<GetMyDonatedItemsViewModel>>(res);
        }
    }
}
