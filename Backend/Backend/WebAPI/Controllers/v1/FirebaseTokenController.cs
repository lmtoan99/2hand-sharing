using Application.DTOs.Firebase;
using Application.Features.FirebaseFeatures.Commands;
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
    public class FirebaseTokenController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> RegisterFirebase([FromBody] FirebaseRequest FirebaseToken)
        {
            return Ok(await Mediator.Send(new RegisterFirebaseCommand
            {
                UserId = GetUserId(),
                FirebaseToken = FirebaseToken.FirebaseToken
            }));
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveFirebase([FromBody] FirebaseRequest request)
        {
            return Ok(await Mediator.Send(new RemoveFirebaseTokenCommand
            {
                UserId = GetUserId(),
                FirebaseToken = request.FirebaseToken
            }));
        }
    }
}
