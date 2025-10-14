using EvilCorpBakery.API.Features.Auth.CreateUser;
using EvilCorpBakery.API.Features.Auth.GetUserJoinDate;
using EvilCorpBakery.API.Features.Auth.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvilCorpBakery.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginUserCommand command)
        {
            var token = await _sender.Send(command);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] CreateUserCommand command)
        {
            var result = await _sender.Send(command);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}/joindate")]
        public async Task<IActionResult> GetUserJoinDate([FromRoute] int id)
        {
            var query = new GetUserJoinDateQuery(id);
            var result = await _sender.Send(query);
            return Ok(result);
        }



        /*        [AllowAnonymous]
                [HttpPost("register/confirmation")]
                public async Task<IActionResult> RegisterConfirmation([FromForm] int numbers)
                {
                    var result = await _sender.Send(numbers);
                    return Ok(result);
                }*/

    }
}
