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
        public IdentityController()
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync()
        {
            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync()
        {
            return Ok();
        }
    }
}
