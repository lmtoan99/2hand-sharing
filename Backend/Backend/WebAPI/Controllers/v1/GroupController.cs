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

        [HttpGet("{groupId}/member")]
        public async Task<IActionResult> GetAllMember([FromQuery] GetAllGroupMemberByGroupIdParameter filter, int groupId)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllGroupMemberByGroupIdQuery()));
            return Ok(await Mediator.Send(new GetAllGroupMemberByGroupIdQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, GroupId = groupId }));
        }

        [HttpDelete("{groupId}/member/{memberId}")]
        public async Task<IActionResult> DeleteMember(int groupId,int memberId)
        {
            return Ok(await Mediator.Send(new DeleteMemberCommand {
                groupId = groupId,
                memberId = memberId,
                adminId = GetUserId()
            }));
        }

        [HttpPost("{groupId}/member")]
        public async Task<IActionResult> AddMember(int groupId, [FromBody] AddMemberRequest request)
        {
            return Ok(await Mediator.Send(new AddMemberCommand
            {
                GroupId = groupId,
                Email = request.Email,
                AdminId = GetUserId()
            }));
        }

        [HttpGet("joined-group")]
        public async Task<IActionResult> GetAllGroupJoined([FromQuery] GetAllGroupJoinedParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllGroupJoinedQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, UserId = this.GetUserId() }));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllGroupParameter filter)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllGroupQuery()));
            return Ok(await Mediator.Send(new GetAllGroupQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }
    }
}
