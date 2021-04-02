using Application.DTOs.Account;
using Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IAccountService _service;
        public IdentityController(IAccountService service)
        {
            this._service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest request)
        {
            return Ok(await _service.RegisterAsync(request));
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthenticateRequest request)
        {
            return Ok(await _service.AuthenticateAsync(request));
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _service.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }
    }
}
