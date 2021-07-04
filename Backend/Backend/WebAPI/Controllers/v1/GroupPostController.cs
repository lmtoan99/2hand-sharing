using Application.DTOs.Comment;
using Application.Features.GroupPostFeatures.Commands;
using Application.Features.GroupPostFeatures.Queries;
using Application.Filter;
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
        
        [HttpGet("{postId}/comment")]
        public async Task<IActionResult> GetListCommentOnPost([FromQuery] RequestParameter request, int postId)
        {
            return Ok(await Mediator.Send(new GetListCommentOnPostQuery { PostId = postId, UserId = GetUserId(),PageNumber = request.PageNumber, PageSize = request.PageSize}));
        }
    }
}
