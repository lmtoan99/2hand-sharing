using Application.DTOs.Item;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
using Application.Features.ReceiveItemInformationFeatures.Commands;
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
    public class ReceiveItemController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateReceiveRequest([FromBody] ReceiveItemRequest request)
        {
            return Ok(await Mediator.Send(new CreateReceiveRequestCommand
            {
                ItemId = request.ItemId,
                ReceiveReason = request.ReceiveReason,
                ReceiverId = this.GetUserId()
            }));
        }
        
        [HttpPut("{requestId}/accept")]
        public async Task<IActionResult> AcceptReceiveRequest(int requestId)
        {
            return Ok(await Mediator.Send(new AcceptReceiveRequestCommand {requestId = requestId, userId = GetUserId() }));
        }

        [HttpPut("{requestId}/confirm-receive")]
        public async Task<IActionResult> ConfirmReceiveItem(int requestId)
        {
            return Ok(await Mediator.Send(new UpdateStatusConfirmReceiveItemCommand { Id = requestId, UserId = this.GetUserId() }));
        }
    }
}
