using Application.DTOs.Item;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
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
            if (filter==null)return Ok(await Mediator.Send(new GetAllPostItemQuery()));
            return Ok(await Mediator.Send(new GetAllPostItemQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostItemRequest item)
        {
            Response<PostItemResponse> itemResponse = await Mediator.Send(new PostItemCommand(){
                ItemName = item.ItemName,
                ReceiveAddress = item.ReceiveAddress,
                CategoryId = item.CategoryId,
                DonateAccountId = int.Parse(HttpContext.User.FindFirst("userid").Value),
                Description = item.Description,
                ImageNumber = item.ImageNumber
                });
            if (itemResponse.Errors != null) return Ok(itemResponse);

            return Ok(itemResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetItemByIdQuery { Id = id }));
        }
    }
}
