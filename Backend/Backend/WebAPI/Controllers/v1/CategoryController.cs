using Application.Features.CategoryFeatures.Queries.GetAllCategories;
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
            if (HttpContext.User.Identity.Name != null)
            {
                Console.WriteLine("1"+HttpContext.User.Identity.Name);
            }
            return Ok(await Mediator.Send(new GetAllCategoriesQuery()));
        }
    }
}
