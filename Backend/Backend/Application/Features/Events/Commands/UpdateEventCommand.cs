using Application.DTOs.Event;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Events.Commands
{
    public class UpdateEventCommand : IRequest<Response<EventDTO>>
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime EndDate { get; set; }
        public string Content { get; set; }
        public string EventName { get; set; }
    }
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateEventCommand, Response<EventDTO>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminRepository;
        private readonly IMapper _mapper;
        public UpdateGroupCommandHandler(IEventRepositoryAsync eventRepository, IMapper mapper, IGroupAdminDetailRepositoryAsync groupAdminRepository)
        {
            _eventRepository = eventRepository;
            _groupAdminRepository = groupAdminRepository;
            _mapper = mapper;
        }
        public async Task<Response<EventDTO>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventInfo = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventInfo == null) throw new KeyNotFoundException();
            var checkAdmin = await _groupAdminRepository.GetByConditionAsync(e => e.AdminId == request.UserId && e.GroupId == eventInfo.GroupId);
            if (checkAdmin.Count == 0)
            {
                throw new ApiException("You are not admin of this group.");
            }
            if (request.Content != null)
            {
                eventInfo.Content = request.Content;
            }
            if (request.EventName != null)
            {
                eventInfo.EventName = request.EventName;
            }
            if (request.EndDate != default)
            {
                eventInfo.EndDate = request.EndDate;
            }

            await _eventRepository.UpdateAsync(eventInfo);
            return new Response<EventDTO>(_mapper.Map<EventDTO>(eventInfo));
        }
    }
}
