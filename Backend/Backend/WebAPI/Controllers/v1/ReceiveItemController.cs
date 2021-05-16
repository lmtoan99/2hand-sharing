using Application.DTOs.Item;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
using Application.Features.ReceiveItemInformationFeatures.Commands;
using Application.Features.ReceiveItemInformationFeatures.Queries;
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
        
        [HttpGet("{requestId}")]
        public async Task<IActionResult> DoneeGetReceiveRequestById(int requestId)
        {
            return Ok(await Mediator.Send(new DoneeGetReceiveRequestByIdQuery {RequestId = requestId, UserId = GetUserId() }));
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

        [HttpPut("{requestId}/cancel-receive")]
        public async Task<IActionResult> CancelReceiveItem(int requestId)
        {
            return Ok(await Mediator.Send(new CancelReceiveItemCommand { Id = requestId, UserId = this.GetUserId() }));
        }

        [HttpDelete("{requestId}/delete-receive")]
        public async Task<IActionResult> Delete(int requestId)
        {
            return Ok(await Mediator.Send(new DeleteRequestReceiveByIdCommand { Id = requestId,UserId=this.GetUserId() }));
        }

        [HttpPut("{requestId}/send-thanks")]
        public async Task<IActionResult> SendThanks(int requestId, [FromBody] SendThanksDTO thanks)
        {
            return Ok(await Mediator.Send(new SendThanksCommand { requestId = requestId,thanks = thanks.thanks, userId = GetUserId()}));
        }

        [HttpGet("my-request")]
        public async Task<IActionResult> DoneeGetListReceiveRequest([FromQuery]RequestParameter filter)
        {
            return Ok(await Mediator.Send(new DoneeGetListReceiveRequestQuery { UserId = GetUserId(),PageNumber = filter.PageNumber, PageSize = filter.PageSize}));
        }
    }
}
