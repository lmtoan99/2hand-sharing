using Application.DTOs.Award;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AwardFeatures.Query
{
    public class GetTopAwardsQuery : IRequest<Response<IEnumerable<GetAwardsViewModel>>>
    {

    }
    public class GetTopAwardsQueryHandler : IRequestHandler<GetTopAwardsQuery, Response<IEnumerable<GetAwardsViewModel>>>
    {
        private readonly IAwardRepositoryAsync _awardRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetTopAwardsQueryHandler(IAwardRepositoryAsync awardRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _awardRepository = awardRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GetAwardsViewModel>>> Handle(GetTopAwardsQuery request, CancellationToken cancellationToken)
        {
            var awards = await _awardRepository.GetTopAwardAsync();
            if (awards == null) throw new KeyNotFoundException("Nobody in award");
            List<GetAwardsViewModel> list = _mapper.Map<List<GetAwardsViewModel>>(awards);
            foreach (GetAwardsViewModel i in list)
            {
                i.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(i.AvatarUrl);
            }
            return new Response<IEnumerable<GetAwardsViewModel>>(list);
        }
    }
}
