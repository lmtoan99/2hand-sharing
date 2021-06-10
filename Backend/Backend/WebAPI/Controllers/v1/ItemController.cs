using Application.DTOs.Item;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
using Application.Features.ReceiveItemInformationFeatures.Commands;
using Application.Filter;
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
            return Ok(await Mediator.Send(new GetItemByIdQuery { Id = id}));
        }

        [HttpGet("{itemId}/receive-request")]
        public async Task<IActionResult> GetListReceiveRequest(int itemId)
        {
            return Ok(await Mediator.Send(new GetListReceiveRequestQuery { ItemId = itemId, UserId = this.GetUserId() }));
        }


        [HttpPut("{itemId}/confirm-send")]
        public async Task<IActionResult> ConfirmSendItem(int itemId)
        {
            return Ok(await Mediator.Send(new UpdateStatusConfirmSendItemCommand { Id = itemId, UserId = this.GetUserId() }));
        }

        [HttpGet("{userId}/donations")]
        public async Task<IActionResult> DonorGetListItemDonate([FromQuery] RequestParameter filter, int userId)
        {
            return Ok(await Mediator.Send(new GetListItemDonateByDonorIdQuery{userId = userId, PageNumber = filter.PageNumber, PageSize = filter.PageSize }));
        }

        [HttpGet("{itemId}/received-user")]
        public async Task<IActionResult> GetReceivedUserInfo(int itemId)
        {
            return Ok(await Mediator.Send(new GetReceivedUserInfoQuery { itemId = itemId }));
        }

        [HttpPut("{itemId}/cancel-donate")]
        public async Task<IActionResult> CancelDonateItem(int itemId)
        {
            return Ok(await Mediator.Send(new UpdateStatusCancelDonateItemCommand { Id = itemId, UserId = this.GetUserId() }));
        }
    }
}
