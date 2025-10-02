using EvilCorpBakery.API.Features.Products.CreateProduct;
using EvilCorpBakery.API.Features.Products.DeleteProduct;
using EvilCorpBakery.API.Features.Products.GetProductById;
using EvilCorpBakery.API.Features.Products.GetProductByName;
using EvilCorpBakery.API.Features.Products.GetProducts;
using EvilCorpBakery.API.Features.Products.GetProductSuggestions;
using EvilCorpBakery.API.Features.Products.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvilCorpBakery.API.Controllers
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var playerId = await _sender.Send(command);

            return Ok(playerId);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        //[Authorize(Roles ="User")]
        [HttpGet("all")]
        public async Task<IActionResult> GetProducts()
        {

            var query = new GetProductsQuery();
            var products = await _sender.Send(query);

            return Ok(products);
        }

        [HttpGet("specified/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdQuery query)
        {
            var product = await _sender.Send(query);
            return Ok(product);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Id from route and command do not match");
            }
            await _sender.Send(command);
            return NoContent();
        }

        /*[HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                var allProductsQuery = new GetProductsQuery();
                var allProducts = await _sender.Send(allProductsQuery);
                return Ok(allProducts);
            }

            GetProductByNameQuery getProductByNameQuery = new(query);
            var product = await _sender.Send(getProductByNameQuery);

            return Ok(product);
        }*/

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetProductSuggestions([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 1)
            {
                return Ok(new { data = new List<object>() });
            }

           var suggestionsQuery = new GetProductSuggestionsQuery(query);


            var suggestions = await _sender.Send(suggestionsQuery);
            return Ok(suggestions);
        }

    }
}
