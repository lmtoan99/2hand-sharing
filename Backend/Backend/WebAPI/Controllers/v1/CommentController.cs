using Application.DTOs.Comment;
using Application.Features.CommentFeatures.Commands;
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
    public class CommentController : BaseApiController
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] PostCommentOnGroupDTO comment)
        {
            return Ok(await Mediator.Send(new UpdateCommentCommand {Content = comment.Content, CommentId = id, UserId = GetUserId()}));
        }
    }
}
