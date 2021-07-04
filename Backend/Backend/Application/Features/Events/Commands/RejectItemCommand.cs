using Application.Enums;
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
    public class RejectItemCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int ItemId { get; set; }
    }
    public class RejectItemCommandHandler : IRequestHandler<RejectItemCommand, Response<string>>
    {
        private readonly IEventRepositoryAsync _eventRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetail;
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IMapper _mapper;

        public RejectItemCommandHandler(IEventRepositoryAsync eventRepository, IGroupAdminDetailRepositoryAsync groupAdminDetail, IItemRepositoryAsync itemRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _groupAdminDetail = groupAdminDetail;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(RejectItemCommand request, CancellationToken cancellationToken)
        {
            var groupEvent = await _eventRepository.GetByIdAsync(request.EventId);
            if (groupEvent == null)
            {
                throw new ApiException("Event not existed");
            }
            var admin = await _groupAdminDetail.GetByConditionAsync(a => a.AdminId == request.UserId && a.GroupId == groupEvent.GroupId);
            if (admin == null)
            {
                throw new ApiException("You are not the admin of this group");
            }
            var item = await _itemRepository.GetItemWithEvent(request.ItemId);
            if (item.DonateEventInformation.EventId != request.EventId)
            {
                throw new ApiException("This item not belong to this event");
            }
            await _itemRepository.DeleteAsync(item);
            return new Response<string>(null);
        }
    }
}
