using EvilCorpBakery.API.Features.Address.Commands;
using EvilCorpBakery.API.Features.Address.Queries.GetAddressByUserId;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EvilCorpBakery.API.Controllers
{
    [ApiController]
    [Route("api/v1/address")]
    public class AddressController : Controller
    {
        private readonly ISender _sender;

        public AddressController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddressByUserId([FromRoute] GetAddressByUserIdQuery query)
        {
            var result = await _sender.Send(query);

            return Ok(result);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO request)
        {
/*            CreateAddressCommand command = new CreateAddressCommand()
            {
                address = new AddressDTO
                {
                    UserId = command.UserId,
                    Street = command.Street,
                    City = command.City,
                    State = command.State,
                    ZipCode = command.ZipCode,
                    Country = command.Country
                } test
            }*/
// obsluzyc
            var result = await _sender.Send(request);
            return Ok(result);
        }

    }
}
