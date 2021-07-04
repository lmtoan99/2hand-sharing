using Application.DTOs.Event;
using Application.DTOs.Item;
using Application.Features.Events.Commands;
using Application.Features.Events.Queries;
using Application.Features.ItemFeatures.Queries;
using Application.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers.v1
{
    [ApiController]
    [Authorize]
    public class EventController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] RequestParameter request)
        {
            return Ok(await Mediator.Send(new GetAllEventsQuery{ PageNumber = request.PageNumber, PageSize = request.PageSize}));
        }
        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(int eventId)
        {
            return Ok(await Mediator.Send(new GetEventByEventIdQuery { EventId = eventId}));
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO request)
        {
            return Ok(await Mediator.Send(
                new CreateEventCommand
                { 
                    Content = request.Content,
                    EndDate = request.EndDate,
                    EventName = request.EventName,
                    GroupId = request.GroupId,
                    StartDate = request.StartDate,
                    UserId = this.GetUserId()
                }));
        }
        [HttpPost("{eventId}/item")]
        public async Task<IActionResult> DonateItemForEvent([FromBody] PostItemRequest item, int eventId)
        {
            return Ok(await Mediator.Send(
                new DonateItemForEventCommand
                {
                    ItemName = item.ItemName,
                    EventId = eventId,
                    ReceiveAddress = item.ReceiveAddress,
                    CategoryId = item.CategoryId,
                    DonateAccountId = this.GetUserId(),
                    Description = item.Description,
                    ImageNumber = item.ImageNumber
                })); ;
        }
        [HttpPut("{eventId}/accept-item/{itemId}")]
        public async Task<IActionResult> AcceptItem(int eventId,int itemId)
        {
            return Ok(await Mediator.Send(
                new AcceptItemCommand
                {
                    UserId = GetUserId(),
                    EventId = eventId,
                    ItemId = itemId,
                   
                })); 
        }
        [HttpPut("{eventId}/cancel-accept/{itemId}")]
        public async Task<IActionResult> CancelAccept(int eventId, int itemId)
        {
            return Ok(await Mediator.Send(
                new CancelAcceptItemCommand
                {
                    UserId = GetUserId(),
                    EventId = eventId,
                    ItemId = itemId,

                }));
        }
        [HttpDelete("{eventId}/reject-item/{itemId}")]
        public async Task<IActionResult> RejectItem(int eventId, int itemId)
        {
            return Ok(await Mediator.Send(
                new RejectItemCommand
                {
                    UserId = GetUserId(),
                    EventId = eventId,
                    ItemId = itemId,

                }));
        }


        [HttpGet("{eventId}/item")]
        public async Task<IActionResult> Get([FromQuery] GetAllItemsParameter filter, int eventId)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllDonateItemForEventQuery()));
            return Ok(await Mediator.Send(new GetAllDonateItemForEventQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, EventId = eventId }));
        }
    }
}
