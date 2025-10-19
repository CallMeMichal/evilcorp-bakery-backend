using EvilCorpBakery.API.Features.Auth.Queries.GetUserJoinDate;
using EvilCorpBakery.API.Features.User.Command.DeleteUser;
using EvilCorpBakery.API.Features.User.Command.UpdateUser;
using EvilCorpBakery.API.Features.User.Queries.GetUsers;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvilCorpBakery.API.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : Controller
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("{id}/joindate")]
        public async Task<IActionResult> GetUserJoinDate([FromRoute] int id)
        {
            var query = new GetUserJoinDateQuery(id);
            var result = await _sender.Send(query);
            return Ok(result);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetUsers()
        {
            var commmand = new GetUsersQuery();
            var result = await _sender.Send(commmand);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _sender.Send(command);
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDTO userDto)
        {
            var command = new UpdateUserCommand(id, userDto);
            var result = await _sender.Send(command);

            return Ok(new { success = true, data = result });
        }


    }
}
