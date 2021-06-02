using Application.DTOs.Message;
using Application.Features.MessageFeatures.Commands;
using Application.Features.MessageFeatures.Queries;
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
    public class MessageController : BaseApiController
    {
        [HttpGet("{userid}")]
        public async Task<IActionResult> GetMessage(int userid, [FromQuery]RequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetMessageQuery { 
                UserGetMessageId = GetUserId(),
                UserMessageWithId = userid,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            }));
        }

        [HttpGet("recent-messages")]
        public async Task<IActionResult> GetRecentMessages([FromQuery] RequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetRecentMessagesQuery
            {
                UserId = GetUserId(),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            }));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageRequest request)
        {
            return Ok(await Mediator.Send(new SendMessageCommand {
                Content = request.Content,
                SendFromAccountId = GetUserId(),
                SendToAccountId = request.SendToAccountId
            }));
        }
    }
}
