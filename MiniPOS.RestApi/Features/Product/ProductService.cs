using Microsoft.EntityFrameworkCore;
using MiniPOS.RestApi.Shared;
using MiniPOS.RestApi.Shared.Model;
using System.Linq;

namespace MiniPOS.RestApi.Features.Product;

public class ProductService : IProductService
{
	private AppDbContext _db;

	public ProductService()
	{
		_db = new AppDbContext();
	}

	public async Task<ProductResponseModel> CreateProduct(ProductModel requestModel)
	{
		ProductResponseModel responseModel = new();

		requestModel.Id = Guid.NewGuid().ToString();
		_db.Products.Add(requestModel);
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

	public async Task<List<ProductModel>> GetProducts(ProductPaginationModel paginationModel)
	{
		List<ProductModel> products;

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.CategoryName is null)
		{
			products = await _db.Products.ToListAsync();

			return products;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.CategoryName is null)
		{
			products = await GetProductsByPagination(paginationModel);

			return products;
		}

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.CategoryName is not null)
		{
			products = await GetProductsByCategory(paginationModel.CategoryName);

			return products;
		}

		products = await GetProductsByCategoryPagination(paginationModel);
		return products;
	}

	private async Task<List<ProductModel>> GetProductsByCategory(string categoryName)
	{
		List<ProductModel> products = new();

		var category = await _db.Categories
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == categoryName);

		if (category is null) return products;

		products = await _db.Products
			.AsNoTracking()
			.Where(x => x.CategoryId == category.Id)
			.ToListAsync();

		return products;
	}

	private async Task<List<ProductModel>> GetProductsByPagination(PaginationModel paginationModel)
	{
		return await _db.Products
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip((paginationModel.Page - 1) * paginationModel.Limit)
					.Take(paginationModel.Limit)
					.ToListAsync();
	}

	private async Task<List<ProductModel>> GetProductsByCategoryPagination(ProductPaginationModel paginationModel)
	{
		List<ProductModel> products = new();

		var category = await _db.Categories
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == paginationModel.CategoryName);

		if (category is null) return products;

		products = await _db.Products
			.AsNoTracking()
			.Where(x => x.CategoryId == category.Id)
			.Skip((paginationModel.Page - 1) * paginationModel.Limit)
			.Take(paginationModel.Limit)
			.ToListAsync();

		return products;
	}

	public async Task<ProductModel> GetProduct(string id)
	{
		var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

		return product!;
	}

	public async Task<ProductResponseModel> UpdateProduct(ProductModel requestModel)
	{
		ProductResponseModel responseModel = new();

		var product = await GetProduct(requestModel.Id!);

		if (product is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Product not found.";
			responseModel.Data = null;
			return responseModel;
		}

		if (requestModel.Name is not null)
		{
			product.Name = requestModel.Name;
		}

		if (requestModel.CategoryId is not null)
		{
			product.CategoryId = requestModel.CategoryId;
		}

		_db.Entry(product).State = EntityState.Modified;
		int result = await _db.SaveChangesAsync();

		bool isSuccessful = result > 0;
		string message = result > 0 ? "Updating successful." : "Updating failed.";
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

	public async Task<ProductResponseModel> DeleteProduct(string id)
	{
		ProductResponseModel responseModel = new();
		var product = await GetProduct(id);

		if (product is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Product not found.";
			responseModel.Data = null;
			return responseModel;
		}

		_db.Entry(product).State = EntityState.Deleted;
		int result = await _db.SaveChangesAsync();

		bool isSuccessful = result > 0;
		string message = result > 0 ? "Deleting successful." : "Deleting failed.";
		responseModel.IsSuccessful = isSuccessful;
		responseModel.Message = message;

		if (!isSuccessful)
		{
			responseModel.Data = null;
			return responseModel;
		}

		responseModel.Data = null;
		return responseModel;
	}
}