using Application.DTOs.Comment;
using Application.Features.GroupPostFeatures.Commands;
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
    public class GroupPostController : BaseApiController
    {
        [HttpPost("{postId}/comment")]
        public async Task<IActionResult> PostCommentOnGroupPost([FromBody] PostCommentOnGroupDTO comment, int postId)
        {
            return Ok(await Mediator.Send(new PostCommentOnGroupPostCommand { PostId = postId, Content = comment.Content, PostByAccontId = GetUserId()}));
        }
    }
}
