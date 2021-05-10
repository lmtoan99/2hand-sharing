using Application.DTOs.Group;
using Application.Features.GroupFeatures.Commands;
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
    public class GroupController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            return Ok(await Mediator.Send(new CreateGroupCommand
            {
                Description = request.Description,
                GroupName = request.GroupName,
                Rules = request.Rules,
                AdminId = GetUserId()
            }));
        }
    }
}
