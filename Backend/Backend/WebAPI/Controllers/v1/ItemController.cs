using Application.DTOs.Item;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
using Application.Features.ReceiveItemInformationFeatures.Commands;
using Application.Wrappers;
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
    public class ItemController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllItemsParameter filter)
        {
            if (filter==null) return Ok(await Mediator.Send(new GetAllPostItemQuery()));
            return Ok(await Mediator.Send(new GetAllPostItemQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostItemRequest item)
        {
            Response<PostItemResponse> itemResponse = await Mediator.Send(new PostItemCommand(){
                ItemName = item.ItemName,
                ReceiveAddress = item.ReceiveAddress,
                CategoryId = item.CategoryId,
                DonateAccountId = this.GetUserId(),
                Description = item.Description,
                ImageNumber = item.ImageNumber
                });

            return Ok(itemResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetItemByIdQuery { Id = id }));
        }

        [HttpPost("{itemId}/receive-request")]
        public async Task<IActionResult> CreateReceiveRequest(int itemId, [FromBody]ReceiveItemRequest request)
        {
            return Ok(await Mediator.Send(new CreateReceiveRequestCommand
            {
                ItemId = itemId,
                ReceiveReason = request.ReceiveReason,
                ReceiverId = this.GetUserId()
            }));
        }

        [HttpGet("{id}/receive-request")]
        public async Task<IActionResult> GetListReceiveRequest(int id)
        {
            return Ok(await Mediator.Send(new GetListReceiveRequestQuery { ItemId = id, UserId = this.GetUserId()}));
        }

        [HttpPut("{itemId}/receive-request/{requestId}/accept")]
        public async Task<IActionResult> AcceptReceiveRequest(int itemId, int requestId)
        {
            return Ok(await Mediator.Send(new AcceptReceiveRequestCommand { itemId = itemId, requestId = requestId, userId = GetUserId() }));
        }

        [HttpPut("{id}/confirm-send-item")]
        public async Task<IActionResult> confirmSendItem(int id)
        {
            return Ok(await Mediator.Send(new UpdateStatusConfirmSendItemCommand { Id = id, UserId = this.GetUserId()}));
        }

        [HttpPut("{id}/confirm-receive-item")]
        public async Task<IActionResult> ConfirmReceiveItem(int id)
        {
            return Ok(await Mediator.Send(new UpdateStatusConfirmReceiveItemCommand { Id = id, UserId = this.GetUserId()}));
        }
    }
}
