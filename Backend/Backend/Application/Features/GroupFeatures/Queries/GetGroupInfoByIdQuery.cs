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
    public class GetGroupInfoByIdQuery : IRequest<Response<GroupDTO>>
    {
        public int id { get; set; }
        public class GetGroupInfoByIdQueryHandle : IRequestHandler<GetGroupInfoByIdQuery, Response<GroupDTO>>
        {
            private readonly IGroupRepositoryAsync _groupRepository;
            private readonly IMapper _mapper;
            public GetGroupInfoByIdQueryHandle(IGroupRepositoryAsync groupRepository, IMapper mapper)
            {
                _groupRepository = groupRepository;
                _mapper = mapper;
            }
            public async Task<Response<GroupDTO>> Handle(GetGroupInfoByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _groupRepository.GetByIdAsync(request.id);
                if (result == null) throw new KeyNotFoundException();
                return new Response<GroupDTO>(_mapper.Map<GroupDTO>(result));
            }
        }
    }
}
