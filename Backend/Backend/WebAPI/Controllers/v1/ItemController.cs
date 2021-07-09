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
        public async Task<IActionResult> Get([FromQuery] GetAllItemsParameter filter, [FromQuery] string query, [FromQuery] int categoryId)
        {
            return Ok(await Mediator.Send(new GetAllPostItemQuery {Query = query, CategoryId = categoryId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
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
            return Ok(await Mediator.Send(new GetItemByIdQuery { Id = id, UserId = GetUserId()}));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateItemDTO item)
        {
            return Ok(await Mediator.Send(new UpdateItemCommand {
                Id = id, ItemName = item.ItemName, CategoryId = item.CategoryId, DeletedImages = item.DeletedImages, Description = item.Description, ImageNumber = item.ImageNumber, ReceiveAddress = item.ReceiveAddress
            }));
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

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> CancelDonateItem(int itemId)
        {
            return Ok(await Mediator.Send(new CancelDonateItemCommand { Id = itemId, UserId = this.GetUserId() }));
        }
    }
}
