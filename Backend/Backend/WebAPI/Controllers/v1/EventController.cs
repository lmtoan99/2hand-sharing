using Application.DTOs.Event;
using Application.Features.Events.Commands;
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
    }
}
