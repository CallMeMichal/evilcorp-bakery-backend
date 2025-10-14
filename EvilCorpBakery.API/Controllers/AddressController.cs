using EvilCorpBakery.API.Features.Address.GetAddressByUserId;
using EvilCorpBakery.API.Features.Orders.GetOrders;
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
    }
}
