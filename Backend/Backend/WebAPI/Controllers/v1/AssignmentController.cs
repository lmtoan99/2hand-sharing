using Application.DTOs.Assignment;
using Application.Features.AssignmentFeatures.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers.v1
{
    public class AssignmentController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> AssignItemToMember([FromBody] AssignMemberDTO assign)
        {
            return Ok(await Mediator.Send(new AssignItemToMemberCommand { 
                AssignByAccountId = GetUserId(),
                AssignedMemberId = assign.AssignedMemberId,
                DonateEventInformationId = assign.DonateEventInformationId,
                ExpirationDate = assign.ExpirationDate,
                Note = assign.Note
            }));
        }
    }
}
