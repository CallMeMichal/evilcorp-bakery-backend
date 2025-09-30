using EvilCorpBakery.API.Features.Auth.CreateUser;
using EvilCorpBakery.API.Features.Auth.LoginUser;
using MediatR;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginUserCommand command)
        {
            var token = await _sender.Send(command);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserCommand command)
        {
            var token = await _sender.Send(command);
            return Ok(token);
        }
    }
}
