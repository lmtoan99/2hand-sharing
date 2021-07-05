using Application.DTOs.Event;
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

namespace Application.Features.Events.Commands
{
    public class CreateEventCommand : CreateEventDTO, IRequest<Response<Event>>
    {
        public int UserId { get; set; }
    }

    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Response<Event>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetail;
        private readonly IMapper _mapper;
        public CreateEventCommandHandler(IEventRepositoryAsync eventRepository, IGroupAdminDetailRepositoryAsync groupAdminDetail, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _groupAdminDetail = groupAdminDetail;
            _mapper = mapper;
        }
        public async Task<Response<Event>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var checkPermission = await _groupAdminDetail.GetInfoGroupAdminDetail(request.GroupId, request.UserId);
            if (checkPermission == null)
            {
                throw new UnauthorizedAccessException("Not have permission");
            }
            Event entity = _mapper.Map<Event>(request);
            entity.StartDate = DateTime.UtcNow;
            var result = await _eventRepository.AddAsync(entity);
            return new Response<Event>(result);
        }
    }
}
