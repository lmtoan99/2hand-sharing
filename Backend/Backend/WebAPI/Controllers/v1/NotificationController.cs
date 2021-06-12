using Application.DTOs.Firebase;
using Application.Features.NotificationFeatures.Queries;
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
    public class NotificationController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] RequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetNotificationsOfUserQuery { UserId = GetUserId(), PageNumber = filter.PageNumber, PageSize = filter.PageSize }));
        }
        
    }
}
