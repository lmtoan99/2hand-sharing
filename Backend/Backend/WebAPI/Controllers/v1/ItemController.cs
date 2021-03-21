using Application.Features.ItemFeatures.Queries;
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
    }
}
