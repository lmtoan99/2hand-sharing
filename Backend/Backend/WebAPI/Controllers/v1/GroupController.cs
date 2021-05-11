using Application.DTOs.Group;
using Application.Features.GroupFeatures.Commands;
using Application.Features.GroupFeatures.Queries;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupInfo(int id)
        {
            return Ok(await Mediator.Send(new GetGroupInfoByIdQuery {  id = id}));
        }
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
