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

	[HttpGet("{categoryName}")]
	public IActionResult GetCategory(string categoryName)
	{
		var category = _categoryService.GetCategory(categoryName);

		if (category is null) return NotFound(category);

		return Ok(category);
	}

	[HttpPatch("{categoryName}")]
	public IActionResult UpdateCategory(string categoryName, CategoryModel requestModel)
	{
		requestModel.Name = categoryName;
		var responseModel = _categoryService.UpdateCategory(requestModel);

		if (!responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}

	[HttpDelete("{categoryName}")]
	public IActionResult DeleteCategory(string categoryName)
	{
		var responseModel = _categoryService.DeleteCategory(categoryName);

		if (responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}
}