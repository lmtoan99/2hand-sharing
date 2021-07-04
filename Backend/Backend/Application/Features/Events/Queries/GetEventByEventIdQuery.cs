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
    public class GetEventByEventIdQuery : IRequest<Response<EventDTO>>
    {
        public int EventId { get; set; }
    }
    public class GetEventByEventIdQueryHandler : IRequestHandler<GetEventByEventIdQuery, Response<EventDTO>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetEventByEventIdQueryHandler(IImageRepository imageRepository,IEventRepositoryAsync eventRepository, IMapper mapper, IReceiveItemInformationRepositoryAsync receiveItemInformationRepository)
        {
            _eventRepository = eventRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<EventDTO>> Handle(GetEventByEventIdQuery query, CancellationToken cancellationToken)
        {
            var eventInfo = await _eventRepository.GetByIdAsync(query.EventId);
            if (eventInfo == null) throw new KeyNotFoundException($"Event Not Found.");
            var eventViewModel = _mapper.Map<EventDTO>(eventInfo);
            eventViewModel.GroupAvatar = _imageRepository.GenerateV4SignedReadUrl(eventViewModel.GroupAvatar);
            return new Response<EventDTO>(eventViewModel);
        }
    }
}
