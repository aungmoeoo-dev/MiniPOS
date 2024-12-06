using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniPOS.RestApi.Shared.Model;
using System.Text.Json.Serialization;

namespace MiniPOS.RestApi.Features.Category;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private CategoryService _categoryService;
	public CategoryController()
	{
		_categoryService = new CategoryService();
	}

	[HttpPost]
	public async Task<IActionResult> CreateCategory([FromBody] CategoryModel requestModel)
	{
		var responseModel = await _categoryService.CreateCategory(requestModel);

		if (responseModel.IsSuccessful) return BadRequest(responseModel);

		return Created("", responseModel);
	}

	[HttpGet]
	public async Task<IActionResult> GetCategories([FromQuery]PaginationModel paginationModel)
	{
		var categories = await _categoryService.GetCategories(paginationModel);

		return Ok(categories);
	}

	[HttpGet("{categoryName}")]
	public async Task<IActionResult> GetCategory(string categoryName)
	{
		var category = await _categoryService.GetCategory(categoryName);

		if (category is null) return NotFound(category);

		return Ok(category);
	}

	[HttpPatch("{categoryName}")]
	public async Task<IActionResult> UpdateCategory(string categoryName, CategoryModel requestModel)
	{
		requestModel.Name = categoryName;
		var responseModel = await _categoryService.UpdateCategory(requestModel);

		if (!responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}

	[HttpDelete("{categoryName}")]
	public async Task<IActionResult> DeleteCategory(string categoryName)
	{
		var responseModel = await _categoryService.DeleteCategory(categoryName);

		if (responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}
}