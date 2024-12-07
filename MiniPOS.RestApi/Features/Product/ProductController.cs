using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Product
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private IProductService _productService;

		public ProductController()
		{
			_productService = new ProductService();
		}

		[HttpPost]
		public IActionResult CreateProduct([FromBody] ProductModel productModel)
		{
			var responseModel = _productService.CreateProduct(productModel);

			if (!responseModel.IsSuccessful) return BadRequest(responseModel);

			return Created("", responseModel);
		}

		[HttpGet]
		public IActionResult GetProducts(
			[FromQuery] ProductPaginationModel paginationModel)
		{
			var products = _productService.GetProducts(paginationModel);

			return Ok(products);
		}

		[HttpGet("{id}")]
		public IActionResult GetProduct(string id)
		{
			var product = _productService.GetProduct(id);

			if (product is null) return NotFound(product);

			return Ok(product);
		}

		[HttpPatch("{id}")]
		public IActionResult UpdateProduct(string id, [FromBody] ProductModel requestModel)
		{
			requestModel.Id = id;

			var responseModel = _productService.UpdateProduct(requestModel);

			if (!responseModel.IsSuccessful) return BadRequest(responseModel);

			return Ok(responseModel);
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteProduct(string id)
		{
			var responseModel = _productService.DeleteProduct(id);

			if (!responseModel.IsSuccessful) return BadRequest(responseModel);

			return Ok(responseModel);
		}
	}
}
