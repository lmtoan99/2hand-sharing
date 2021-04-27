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
    public class DoneeGetListReceiveRequestQuery : IRequest<Response<IReadOnlyCollection<ReceiveRequestDoneeViewModel>>>
    {
        public int UserId { get; set; }
    }
    public class DoneeGetListReceiveRequestQueryHandler : IRequestHandler<DoneeGetListReceiveRequestQuery, Response<IReadOnlyCollection<ReceiveRequestDoneeViewModel>>>
    {
        private readonly IReceiveItemInformationRepositoryAsync _receiveItemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public DoneeGetListReceiveRequestQueryHandler(IReceiveItemInformationRepositoryAsync receiveItemRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _receiveItemRepository = receiveItemRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<IReadOnlyCollection<ReceiveRequestDoneeViewModel>>> Handle(DoneeGetListReceiveRequestQuery request, CancellationToken cancellationToken)
        {
            var list = await _receiveItemRepository.GetAllWithItemInfoByAccountId(request.UserId);
            var result = _mapper.Map<List<ReceiveRequestDoneeViewModel>>(list);

            result.ForEach(i => {
                i.ItemImageUrl = _imageRepository.GenerateV4SignedReadUrl(i.ItemImageUrl);
            });
            return new Response<IReadOnlyCollection<ReceiveRequestDoneeViewModel>>(result);
        }
    }
}
