using EvilCorpBakery.API.Features.Category.Command.CreateCategory;
using EvilCorpBakery.API.Features.Category.Command.SetInvisibleCategory;
using EvilCorpBakery.API.Features.Category.Command.SetVisibleCategory;
using EvilCorpBakery.API.Features.Category.Queries.GetCategory;
using EvilCorpBakery.API.Features.Category.Queries.GetVisibleCategory;
using EvilCorpBakery.API.Features.Products.Command.CreateProduct;
using EvilCorpBakery.API.Features.Products.Command.DeleteProduct;
using EvilCorpBakery.API.Features.Products.Command.UpdateProduct;
using EvilCorpBakery.API.Features.Products.Queries.GetProductById;
using EvilCorpBakery.API.Features.Products.Queries.GetProducts;
using EvilCorpBakery.API.Features.Products.Queries.GetProductSuggestions;
using EvilCorpBakery.API.Features.Products.Queries.GetVisibleProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvilCorpBakery.API.Controllers
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var result = await _sender.Send(command);

            return Ok(result);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        //[Authorize(Roles ="User")]
        [HttpGet("all/visible")]
        public async Task<IActionResult> GetVisibleProducts()
        {

            var query = new GetVisibleProductsQuery();
            var products = await _sender.Send(query);

            return Ok(products);
        }

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

        [Authorize(Roles = "Admin")]
        [HttpPost("create/category")]
        public async Task<IActionResult> CreateCategory([FromQuery] string query)
        {

            var command = new CreateCategoryCommand(query);
            var suggestions = await _sender.Send(command);
            return Ok(suggestions);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("invisible/category/{id}")]
        public async Task<IActionResult> SetInvisibleCategory([FromRoute] int id)
        {
            var command = new SetInvisibleCategoryCommand(id);
            var suggestions = await _sender.Send(command);
            return Ok(suggestions);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("visible/category/{id}")]
        public async Task<IActionResult> SetVisibleCategory([FromRoute] int id)
        {
            var command = new SetVisibleCategoryCommand(id);
            var suggestions = await _sender.Send(command);
            return Ok(suggestions);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("category/admin/all")]
        public async Task<IActionResult> GetAllCategoryAdmin()
        { 

            var query = new GetCategoryQuery();
            var products = await _sender.Send(query);

            return Ok(products);
        }

        
        [HttpGet("category/user/all")]
        public async Task<IActionResult> GetVisibleCategory()
        {

              var query = new GetVisibleCategoryQuery();
            var products = await _sender.Send(query);

            return Ok(products);
        }
    }
}
