using Application.DTOs.Group;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Queries
{
    public class GetAllGroupQuery : IRequest<Response<IEnumerable<GroupViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, Response<IEnumerable<GroupViewModel>>>
    {
        private readonly IGroupRepositoryAsync _groupRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupQueryHandler(IGroupRepositoryAsync groupRepositoryAsync, IMapper mapper)
        {
            _groupRepositoryAsync = groupRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<GroupViewModel>>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupParameter>(request);
            var res = await _groupRepositoryAsync.GetAllGroupAsync(validFilter.PageNumber, validFilter.PageSize);
            List<GroupViewModel> list = _mapper.Map<List<GroupViewModel>>(res);
            return new Response<IEnumerable<GroupViewModel>>(list);
        }
    }
}

