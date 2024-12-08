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
		public IActionResult CreateProduct([FromBody] ProductPublicRequestModel requestModel)
		{
			var responseModel = _productService.CreateProduct(requestModel);

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

		[HttpGet("{productCode}")]
		public IActionResult GetProduct(string productCode)
		{
			var product = _productService.GetProduct(productCode);

			if (product is null) return NotFound(product);

			return Ok(product);
		}

		[HttpPatch("{productCode}")]
		public IActionResult UpdateProduct(string productCode, [FromBody] ProductPublicRequestModel requestModel)
		{
			requestModel.Code = productCode;

			var responseModel = _productService.UpdateProduct(requestModel);

			if (!responseModel.IsSuccessful) return BadRequest(responseModel);

			return Ok(responseModel);
		}

		[HttpDelete("{productCode}")]
		public IActionResult DeleteProduct(string productCode)
		{
			var responseModel = _productService.DeleteProduct(productCode);

			if (!responseModel.IsSuccessful) return BadRequest(responseModel);

			return Ok(responseModel);
		}
	}
}
