using Application.DTOs.Event;
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

namespace Application.Features.Events.Queries
{
    public class GetAllEventsQuery : RequestParameter, IRequest<PagedResponse<IReadOnlyCollection<EventDTO>>>
    {
    }
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, PagedResponse<IReadOnlyCollection<EventDTO>>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IMapper _mapper;
        public GetAllEventsQueryHandler(IEventRepositoryAsync eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyCollection<EventDTO>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize);
            var response = _mapper.Map<IReadOnlyCollection<EventDTO>>(result);
            return new PagedResponse<IReadOnlyCollection<EventDTO>>(response, request.PageNumber, request.PageSize);
        }
    }
}
