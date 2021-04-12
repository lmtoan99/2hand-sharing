using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected int GetUserId()
        {
            try
            {
                return int.Parse(HttpContext.User.FindFirst("uid").Value);
            }catch (Exception ex)
            {
                throw new ApiException("Bad JWToken");
            }
        }
    }
}
