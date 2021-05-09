using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Queries.GetAllCategories;
using Application.Features.ItemFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers.v1
{
    [ApiController]
    [Authorize]
    public class CategoryController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllCategoriesQuery()));
        }

        [HttpGet("{categoryid}")]
        public async Task<IActionResult> Get([FromQuery] GetAllItemsParameter filter,int categoryid)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllItemByCategoryIdQuery()));
            return Ok(await Mediator.Send(new GetAllItemByCategoryIdQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, CategoryId=categoryid }));
        }
    }
}
