using Application.DTOs.Assignment;
using Application.Filter;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AssignmentFeatures.Queries
{
    public class GetAllAssignmentByEventIdCommand : RequestParameter, IRequest<PagedResponse<IReadOnlyCollection<AssignmentViewDTO>>>
    {
        public int eventId;
    }
    public class GetAllAssignmentByEventIdCommandHandler : IRequestHandler<GetAllAssignmentByEventIdCommand, PagedResponse<IReadOnlyCollection<AssignmentViewDTO>>>
    {
        private readonly IAssignmentRepositoryAsync _assignmentRepository;
        private readonly IMapper _mapper;
        public GetAllAssignmentByEventIdCommandHandler(IAssignmentRepositoryAsync assignmentRepository, IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyCollection<AssignmentViewDTO>>> Handle(GetAllAssignmentByEventIdCommand request, CancellationToken cancellationToken)
        {
            var list = await _assignmentRepository.GetPagedAssignmentByEventIdAsync(request.eventId, request.PageNumber, request.PageSize);
            var result = _mapper.Map<IReadOnlyCollection<AssignmentViewDTO>>(list);
            return new PagedResponse<IReadOnlyCollection<AssignmentViewDTO>>(result, request.PageNumber, request.PageSize);
        }
    }
}
