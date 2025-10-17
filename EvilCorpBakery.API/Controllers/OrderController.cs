using EvilCorpBakery.API.Features.Orders.Command.CreateOrder;
using EvilCorpBakery.API.Features.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EvilCorpBakery.API.Controllers
{
    [ApiController]
    [Route("api/v1/order")]
    public class OrderController : Controller
    {
        private readonly ISender _sender;

        public OrderController(ISender sender)
        {
            _sender = sender;
        }
/*
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQuery query)
        {
            var result = await _sender.Send(query);

            return Ok(result);
        }*/

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] GetOrdersQuery query)
        {
            var result = await _sender.Send(query);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _sender.Send(command);

            return Ok(result);
        }
    }
}
