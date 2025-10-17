using EvilCorpBakery.API.Features.Auth.GetUserJoinDate;
using EvilCorpBakery.API.Features.User.UpdateUser;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var commmand = new Features.User.GetUsers.GetUsersQuery();
            var result = await _sender.Send(commmand);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var command = new Features.User.DeleteUser.DeleteUserCommand(id);
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
