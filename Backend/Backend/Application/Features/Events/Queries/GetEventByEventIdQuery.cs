using Application.DTOs.Event;
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
    public class GetEventByEventIdQuery : IRequest<Response<GetEventByEventIdViewModel>>
    {
        public int EventId { get; set; }
    }
    public class GetEventByEventIdQueryHandler : IRequestHandler<GetEventByEventIdQuery, Response<GetEventByEventIdViewModel>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IMapper _mapper;
        public GetEventByEventIdQueryHandler(IEventRepositoryAsync eventRepository, IMapper mapper, IReceiveItemInformationRepositoryAsync receiveItemInformationRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }
        public async Task<Response<GetEventByEventIdViewModel>> Handle(GetEventByEventIdQuery query, CancellationToken cancellationToken)
        {
            var eventInfo = await _eventRepository.GetByIdAsync(query.EventId);
            if (eventInfo == null) throw new KeyNotFoundException($"Event Not Found.");
            var eventViewModel = _mapper.Map<GetEventByEventIdViewModel>(eventInfo);           
            return new Response<GetEventByEventIdViewModel>(eventViewModel);
        }
    }
}
