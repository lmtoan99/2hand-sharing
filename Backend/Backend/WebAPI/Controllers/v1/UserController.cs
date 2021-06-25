using Application.DTOs.Account;
using Application.Features.AccountsFeature.Queries;
using Application.Features.AwardFeatures.Query;
using Application.Features.ReceiveItemInformationFeatures.Queries;
using Application.Features.UserFeature.Commands;
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
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo(UpdateUserDTO request)
        {
            return Ok(await Mediator.Send(new UpdateUserInfoCommand {
                UserId = GetUserId(),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Dob = request.Dob,
                Address = request.Address
            }));
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            return Ok(await Mediator.Send(new GetUserInfoByIdQuery { UserId = userId }));
        }

        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateUserAvatar()
        {
            return Ok(await Mediator.Send(new UpdateUserAvatarCommand
            {
                UserId = GetUserId()
            }));
        }

        [HttpGet("/ward")]
        public async Task<IActionResult> GetTopAward()
        {
            return Ok(await Mediator.Send(new GetTopAwardsQuery {}));
        }
    }
}
