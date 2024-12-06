using Microsoft.EntityFrameworkCore;
using MiniPOS.RestApi.Shared;
using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Category;

public class CategoryService
{
	private AppDbContext _db;

	public CategoryService()
	{
		_db = new AppDbContext();
	}

	public async Task<CategoryResponseModel> CreateCategory(CategoryModel requestModel)
	{
		CategoryResponseModel responseModel = new();

		if (requestModel.Name is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Data = null;

			return responseModel;
		}

		requestModel.Id = Guid.NewGuid().ToString();
		_db.Categories.Add(requestModel);
		int result = await _db.SaveChangesAsync();

		bool isSuccessful = result > 0;
		string message = result > 0 ? "Saving successful." : "Saving failed.";
		responseModel.IsSuccessful = isSuccessful;
		responseModel.Message = message;

		if (!isSuccessful)
		{
			responseModel.Data = null;
			return responseModel;
		}

		responseModel.Data = requestModel;
		return responseModel;
	}

	public async Task<List<CategoryModel>> GetCategories(PaginationModel paginationModel)
	{
		List<CategoryModel> list = new();

		if(paginationModel.Page != 0 && paginationModel.Limit != 0)
		{
			list = await _db.Categories
			.AsNoTracking()
			.OrderBy(x => x.Id)
			.Skip((paginationModel.Page - 1) * paginationModel.Limit)
			.Take(paginationModel.Limit)
			.ToListAsync();
			return list;
		}

		list = await _db.Categories
				.AsNoTracking()
				.ToListAsync();
		return list;
	}

	public async Task<CategoryModel> GetCategory(string categoryName)
	{
		var category = await _db.Categories
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == categoryName);

		return category!;
	}

	public async Task<CategoryResponseModel> UpdateCategory(CategoryModel requestModel)
	{
		CategoryResponseModel responseModel = new();
		var category = await _db.Categories
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == requestModel.Name);

		if (category is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Data = null;
			return responseModel;
		}

		if(!string.IsNullOrEmpty(requestModel.Name))
		{
			category.Name = requestModel.Name;
		}
		
		if(!string.IsNullOrEmpty(requestModel.Description))
		{
			category.Description = requestModel.Description;
		}

		_db.Entry(category).State = EntityState.Modified;
		var result = await _db.SaveChangesAsync();

		bool isSuccessful = result > 0;
		string message = result > 0 ? "Updating successful." : "Updating failed.";
		responseModel.IsSuccessful = isSuccessful;
		responseModel.Message = message;

		if (!isSuccessful)
		{
			responseModel.Data = null;
			return responseModel;
		}

		responseModel.Data = category;
		return responseModel;
	}

	public async Task<CategoryResponseModel> DeleteCategory(string categoryName)
	{
		CategoryResponseModel responseModel = new();

		var category = await _db.Categories
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == categoryName);

		int productCount = await _db.Products
			.Where(x => x.CategoryId == category.Id)
			.CountAsync();

		if(productCount < 0)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Category Cannot be Deleted. Related products exist.";
			return responseModel ;
		}

		_db.Entry(category).State = EntityState.Deleted;
		int result = await _db.SaveChangesAsync();

		string message = result > 0 ? "Deleting successful." : "Deleting failed";

		responseModel.IsSuccessful = result > 0;
		responseModel.Message = message;

		return responseModel;
	}
}
