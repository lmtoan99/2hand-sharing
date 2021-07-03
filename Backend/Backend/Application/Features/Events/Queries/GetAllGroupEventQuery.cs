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
    public class GetAllGroupEventQuery : RequestParameter, IRequest<PagedResponse<IReadOnlyCollection<GetEventByEventIdViewModel>>>
    {
        public int GroupId { get; set; }
    }
    public class GetAllGroupEventQueryHandler : IRequestHandler<GetAllGroupEventQuery, PagedResponse<IReadOnlyCollection<GetEventByEventIdViewModel>>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IMapper _mapper;
        public GetAllGroupEventQueryHandler(IEventRepositoryAsync eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyCollection<GetEventByEventIdViewModel>>> Handle(GetAllGroupEventQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventRepository.GetAllGroupEventByGroupIdAsync(request.PageNumber, request.PageSize, request.GroupId);
            var response = _mapper.Map<IReadOnlyCollection<GetEventByEventIdViewModel>>(result);
            return new PagedResponse<IReadOnlyCollection<GetEventByEventIdViewModel>>(response, request.PageNumber, request.PageSize);
        }
    }
}
