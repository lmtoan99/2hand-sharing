using Application.DTOs.Comment;
using Application.DTOs.GroupPost;
using Application.Features.GroupPostFeatures.Commands;
using Application.Features.GroupPostFeatures.Queries;
using Application.Features.PostGroupFeatures.Commands;
using Application.Features.PostGroupFeatures.Queries;
using Application.Filter;
using Application.Wrappers;
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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupPostRequest groupPost)
        {
            return Ok(await Mediator.Send(new CreatePostInGroupCommand()
            {
                Content = groupPost.Content,
                GroupId = groupPost.GroupId,
                PostByAccountId = GetUserId(),
                Visibility = groupPost.Visibility,
                ImageNumber = groupPost.ImageNumber
            }));
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupPost([FromQuery] GetAllGroupPostsParameter filter)
        {
            if (filter == null) return Ok(await Mediator.Send(new GetAllGroupPostsQuery { GroupId = filter.GroupId, PostByAccountId = GetUserId()}));
            return Ok(await Mediator.Send(new GetAllGroupPostsQuery { PageSize = filter.PageSize, PageNumber = filter.PageNumber, GroupId = filter.GroupId, PostByAccountId = GetUserId() }));
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetGroupPostDetail(int postId)
        {
            return Ok(await Mediator.Send(new GetGroupPostByPostIdQuery {PostId = postId, UserId= GetUserId() }));
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdateGroupPost(int postId, [FromBody] UpdateGroupPostRequest groupPost)
        {
            return Ok(await Mediator.Send(new UpdateGroupPostCommand {Content =  groupPost.Content, Visibility = groupPost.Visibility, ImageNumber = groupPost.ImageNumber,DeletedImages = groupPost.DeletedImages, PostId = postId, UserId = GetUserId()}));
        }

        [HttpPost("{postId}/comment")]
        public async Task<IActionResult> PostCommentOnGroupPost([FromBody] PostCommentOnGroupDTO comment, int postId)
        {
            return Ok(await Mediator.Send(new PostCommentOnGroupPostCommand { PostId = postId, Content = comment.Content, PostByAccontId = GetUserId()}));
        }
        
        [HttpGet("{postId}/comment")]
        public async Task<IActionResult> GetListCommentOnPost([FromQuery] RequestParameter request, int postId)
        {
            if (request == null)
            {
                request = new RequestParameter();
            }
            return Ok(await Mediator.Send(new GetListCommentOnPostQuery { PostId = postId, UserId = GetUserId(),PageNumber = request.PageNumber, PageSize = request.PageSize}));
        }
    }
}
