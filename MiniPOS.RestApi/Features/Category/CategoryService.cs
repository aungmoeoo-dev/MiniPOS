using Microsoft.EntityFrameworkCore;
using MiniPOS.RestApi.Shared;
using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Category;

public class CategoryService : ICategoryService
{
	private AppDbContext _db;

	public CategoryService()
	{
		_db = new AppDbContext();
	}

	public CategoryResponseModel CreateCategory(CategoryModel requestModel)
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
		int result = _db.SaveChanges();

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

	public List<CategoryModel> GetCategories(PaginationModel paginationModel)
	{
		List<CategoryModel> list = new();

		if (paginationModel.Page != 0 && paginationModel.Limit != 0)
		{
			list = _db.Categories
			.AsNoTracking()
			.OrderBy(x => x.Code)
			.Skip((paginationModel.Page - 1) * paginationModel.Limit)
			.Take(paginationModel.Limit)
			.ToList();
			return list;
		}

		list = _db.Categories
				.AsNoTracking()
				.ToList();
		return list;
	}

	public CategoryModel GetCategory(string categoryCode)
	{
		var category = _db.Categories
			.AsNoTracking()
			.FirstOrDefault(x => x.Code == categoryCode);

		return category;
	}

	public CategoryResponseModel UpdateCategory(CategoryModel requestModel)
	{
		CategoryResponseModel responseModel = new();

		var category = _db.Categories
			.AsNoTracking()
			.FirstOrDefault(x => x.Code == requestModel.Code);

		if (category is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Category not found.";
			responseModel.Data = null;
			return responseModel;
		}

		if (!string.IsNullOrEmpty(requestModel.Code))
		{
			category.Code = requestModel.Code;
		}

		if (!string.IsNullOrEmpty(requestModel.Name))
		{
			category.Name = requestModel.Name;
		}

		if (!string.IsNullOrEmpty(requestModel.Description))
		{
			category.Description = requestModel.Description;
		}

		_db.Entry(category).State = EntityState.Modified;
		var result = _db.SaveChanges();

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

	public CategoryResponseModel DeleteCategory(string categoryCode)
	{
		CategoryResponseModel responseModel = new();

		var category = _db.Categories
			.AsNoTracking()
			.FirstOrDefault(x => x.Code == categoryCode);

		if (category is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Category not found.";
			return responseModel;
		}

		int productCount = _db.Products
			.Where(x => x.CategoryId == category.Id)
			.Count();

		if (productCount > 0)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Category Cannot be Deleted. Related products exist.";
			return responseModel;
		}

		_db.Entry(category).State = EntityState.Deleted;
		int result = _db.SaveChanges();

		string message = result > 0 ? "Deleting successful." : "Deleting failed";

		responseModel.IsSuccessful = result > 0;
		responseModel.Message = message;

		return responseModel;
	}
}
