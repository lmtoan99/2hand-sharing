using Application.DTOs.Group;
using Application.DTOs.GroupPost;
using Application.Features.Events.Queries;
using Application.Features.GroupFeatures.Commands;
using Application.Features.GroupFeatures.Queries;
using Application.Features.PostGroupFeatures.Commands;
using Application.Features.PostGroupFeatures.Queries;
using Application.Filter;
using Application.Wrappers;
using MediatR;
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
            return Ok(await Mediator.Send(new GetGroupInfoByIdQuery { id = id }));
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

        [HttpPut("{groupId}/demote-admin/{adminId}")]
        public async Task<IActionResult> DemoteAdmin(int groupId, int adminId)
        {
            return Ok(await Mediator.Send(new DemoteAdminCommand
            {
                GroupId = groupId,
                UserId = GetUserId(),
                AdminId = adminId
            }));
        }

        [HttpPut("{groupId}/appoint-admin/{memberId}")]
        public async Task<IActionResult> AppointAdmin(int groupId, int memberId)
        {
            return Ok(await Mediator.Send(new AppointAdminCommand
            {
                GroupId = groupId,
                UserId = GetUserId(),
                MemberId = memberId
            }));
        }
        [HttpPut("{groupId}/join-request/{memberId}/accept")]
        public async Task<IActionResult> AcceptJoinRequest(int groupId, int memberId)
        {
            return Ok(await Mediator.Send(new AcceptJoinRequestCommand
            {
                GroupId = groupId,
                UserId = GetUserId(),
                MemberId = memberId
            }));
        }


        [HttpPut("{groupId}/join-request/{memberId}/reject")]
        public async Task<IActionResult> RejectJoinRequest(int groupId, int memberId)
        {
            return Ok(await Mediator.Send(new RejectJoinRequestCommand
            {
                GroupId = groupId,
                UserId = GetUserId(),
                MemberId = memberId
            }));
        }
        [HttpPut("{groupId}/accept-invitation")]
        public async Task<IActionResult> AcceptInvitation(int groupId)
        {
            return Ok(await Mediator.Send(new AcceptInvitationCommand
            {
                GroupId = groupId,
                UserId = GetUserId(),
            }));
        }

        [HttpPut("{groupId}/decline-invitation")]
        public async Task<IActionResult> DeclineInvitation(int groupId)
        {
            return Ok(await Mediator.Send(new DeclineInvitationCommand
            {
                GroupId = groupId,
                UserId = GetUserId(),
            }));
        }

        [HttpDelete("{groupId}/member/{memberId}")]
        public async Task<IActionResult> DeleteMember(int groupId, int memberId)
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
                UserId = request.UserId,
                AdminId = GetUserId()
            }));
        }

        [HttpGet("joined-group")]
        public async Task<IActionResult> GetAllGroupJoined([FromQuery] GetAllGroupJoinedParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllGroupJoinedQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, UserId = this.GetUserId() }));
        }

        [HttpGet("invitations")]
        public async Task<IActionResult> GetGroupInvitations([FromQuery] RequestParameter filter)
        {
            return Ok(await Mediator.Send(new GetGroupInvitationQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, UserId = this.GetUserId() }));
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RequestParameter
            filter, [FromQuery] string query)
        {
            return Ok(await Mediator.Send(new GetAllGroupQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpPost("{id}/join")]
        public async Task<IActionResult> RequestJoinGroup(int id)
        {
            return Ok(await Mediator.Send(new RequestJoinGroupCommand { 
                GroupId = id,
                UserId = this.GetUserId()
            }));
        }

        [HttpPut("{groupId}/update-avatar")]
        public async Task<IActionResult> UpdateGroupAvatar(int groupId)
        {
            return Ok(await Mediator.Send(new UpdateGroupAvatarCommand
            {
                UserId = GetUserId(),
                GroupId = groupId
            }));
        }

        [HttpGet("{groupId}/get-role")]
        public async Task<IActionResult> GetRoleMemberInGroup([FromQuery] int userId, int groupId)
        {
            return Ok(await Mediator.Send(new GetRoleMemberInGroupQuery { GroupId = groupId, UserId = userId}));
        }

        [HttpGet("{groupId}/join-status")]
        public async Task<IActionResult> GetJoinStatus(int groupId)
        {
            return Ok(await Mediator.Send(new GetJoinStatusQuery { GroupId = groupId, UserId = GetUserId() }));
        }

        [HttpGet("{groupId}/admin")]
        public async Task<IActionResult> GetListAdmin([FromQuery] RequestParameter request,int groupId)
        {
            return Ok(await Mediator.Send(new GetAllGroupAdminQuery {
            GroupId = groupId,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize}));
        }

        [HttpGet("{groupId}/request-join")]
        public async Task<IActionResult> GetListJoinGroupRequest([FromQuery] RequestParameter request, int groupId)
        {
            return Ok(await Mediator.Send(new GetListJoinRequestQuery
            {
                AdminId = GetUserId(),
                GroupId = groupId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            })); ;
        }

        [HttpGet("{groupId}/event")]
        public async Task<IActionResult> Get([FromQuery] RequestParameter filter, int groupId)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllGroupEventQuery()));
            return Ok(await Mediator.Send(new GetAllGroupEventQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, GroupId = groupId }));
        }

        [HttpDelete("{groupId}/leave")]
        public async Task<IActionResult> LeftOutOfGroup(int groupId)
        {
            return Ok(await Mediator.Send(new LeftOutOfGroupCommand
            {
                groupId = groupId,
                memberId = GetUserId()
            }));
        }
    }
}
