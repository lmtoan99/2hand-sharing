using Application.Features.AccountsFeature.Queries;
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
    public class UserController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            return Ok(await Mediator.Send(new GetUserInfoQuery { UserId = GetUserId() }));
        }
    }
}
