using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniPOS.RestApi.Shared.Model;
using System.Text.Json.Serialization;

namespace MiniPOS.RestApi.Features.Category;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private ICategoryService _categoryService;
	public CategoryController()
	{
		_categoryService = new CategoryService();
	}

	[HttpPost]
	public IActionResult CreateCategory([FromBody] CategoryModel requestModel)
	{
		var responseModel = _categoryService.CreateCategory(requestModel);

		if (responseModel.IsSuccessful) return BadRequest(responseModel);

		return Created("", responseModel);
	}

	[HttpGet]
	public IActionResult GetCategories([FromQuery]PaginationModel paginationModel)
	{
		var categories = _categoryService.GetCategories(paginationModel);

		return Ok(categories);
	}

	[HttpGet("{categoryCode}")]
	public IActionResult GetCategory(string categoryCode)
	{
		var category = _categoryService.GetCategory(categoryCode);

		if (category is null) return NotFound(category);

		return Ok(category);
	}

	[HttpPatch("{categoryCode}")]
	public IActionResult UpdateCategory(string categoryCode, CategoryModel requestModel)
	{
		requestModel.Code = categoryCode;
		var responseModel = _categoryService.UpdateCategory(requestModel);

		if (!responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}

	[HttpDelete("{categoryCode}")]
	public IActionResult DeleteCategory(string categoryCode)
	{
		var responseModel = _categoryService.DeleteCategory(categoryCode);

		if (responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}
}