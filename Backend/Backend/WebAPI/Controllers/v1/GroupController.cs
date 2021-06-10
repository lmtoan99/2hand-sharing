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

        [HttpGet("{groupid}/get-group-member")]
        public async Task<IActionResult> GetAllMember([FromQuery] GetAllGroupMemberByGroupIdParameter filter, int groupid)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllGroupMemberByGroupIdQuery()));
            return Ok(await Mediator.Send(new GetAllGroupMemberByGroupIdQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, GroupId = groupid }));
        }

        [HttpPost("add-member")]
        public async Task<IActionResult> AddMember([FromBody] AddMemberRequest request)
        {
            return Ok(await Mediator.Send(new AddMemberCommand
            {
                GroupId = request.GroupId,
                Email = request.Email,
                AdminId = GetUserId()
            }));
        }

        [HttpGet("get-joined-group")]
        public async Task<IActionResult> GetAllGroupJoined()
        {
            return Ok(await Mediator.Send(new GetAllGroupJoinedQuery { UserId = this.GetUserId() }));
        }
    }
}
