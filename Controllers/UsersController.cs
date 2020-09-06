using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleStore.DB;
using SimpleStore.Models;
using SimpleStore.Services;

namespace SimpleStore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AuthenticateResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("current")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrent()
        {
            var user = await _userService.GetCurrent();
            if (user == null)
                return BadRequest(new { message = "Current user was not found" });

            return Ok(user);
        }
    }
}
