using Application.DTOs.Firebase;
using Application.DTOs.Notification;
using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.NotificationFeatures.Queries
{
    public class GetNotificationsOfUserQuery : IRequest<PagedResponse<IReadOnlyCollection<NotificationDTO>>>
    {
        public int UserId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetNotificationsOfUserQueryHandler : IRequestHandler<GetNotificationsOfUserQuery, PagedResponse<IReadOnlyCollection<NotificationDTO>>>
    {
        private readonly INotificationRepositoryAsync _notificationRepositoryAsync;
        private readonly IMapper _mapper;

        public GetNotificationsOfUserQueryHandler(INotificationRepositoryAsync notificationRepositoryAsync, IMapper mapper)
        {
            _notificationRepositoryAsync = notificationRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IReadOnlyCollection<NotificationDTO>>> Handle(GetNotificationsOfUserQuery request, CancellationToken cancellationToken)
        {
            var list = await _notificationRepositoryAsync.GetNotificationsOfUser(request.UserId, request.PageNumber, request.PageSize * 2);
            var notifications = _mapper.Map<List<NotificationDTO>>(list);
            return new PagedResponse<IReadOnlyCollection<NotificationDTO>>(_mapper.Map<IReadOnlyCollection<NotificationDTO>>(notifications), request.PageNumber, request.PageSize);
        }
    }
}
