using Application.DTOs.Event;
using Application.Filter;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
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
        public string Query { get; set; }
    }
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, PagedResponse<IReadOnlyCollection<EventDTO>>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public GetAllEventsQueryHandler(IEventRepositoryAsync eventRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }
        public async Task<PagedResponse<IReadOnlyCollection<EventDTO>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Event> result;
            if (request.Query != null)
            {
                result = await _eventRepository.SearchEventPagedResponse(request.Query, request.PageNumber, request.PageSize);
            }
            else
            {
                result = await _eventRepository.GetAllEventPagedResponse(request.PageNumber, request.PageSize);
            }
            var response = _mapper.Map<List<EventDTO>>(result);
            response.ForEach(e =>
            {
                e.GroupAvatar = this._imageRepository.GenerateV4SignedReadUrl(e.GroupAvatar);
            });

            return new PagedResponse<IReadOnlyCollection<EventDTO>>(response, request.PageNumber, request.PageSize);
        }
    }
}
