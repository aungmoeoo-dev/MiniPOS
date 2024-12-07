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

	public ProductResponseModel CreateProduct(ProductModel requestModel)
	{
		ProductResponseModel responseModel = new();

		requestModel.Id = Guid.NewGuid().ToString();
		_db.Products.Add(requestModel);
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

	public List<ProductModel> GetProducts(ProductPaginationModel paginationModel)
	{
		List<ProductModel> products;

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.CategoryName is null)
		{
			products = _db.Products.ToList();

			return products;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.CategoryName is null)
		{
			products = GetProductsByPagination(paginationModel);

			return products;
		}

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.CategoryName is not null)
		{
			products = GetProductsByCategory(paginationModel.CategoryName);

			return products;
		}

		products = GetProductsByCategoryPagination(paginationModel);
		return products;
	}

	private List<ProductModel> GetProductsByCategory(string categoryName)
	{
		List<ProductModel> products = new();

		var category = _db.Categories
			.AsNoTracking()
			.FirstOrDefault(x => x.Name == categoryName);

		if (category is null) return products;

		products = _db.Products
			.AsNoTracking()
			.Where(x => x.CategoryId == category.Id)
			.ToList();

		return products;
	}

	private List<ProductModel> GetProductsByPagination(PaginationModel paginationModel)
	{
		return _db.Products
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip((paginationModel.Page - 1) * paginationModel.Limit)
					.Take(paginationModel.Limit)
					.ToList();
	}

	private List<ProductModel> GetProductsByCategoryPagination(ProductPaginationModel paginationModel)
	{
		List<ProductModel> products = new();

		var category = _db.Categories
			.AsNoTracking()
			.FirstOrDefault(x => x.Name == paginationModel.CategoryName);

		if (category is null) return products;

		products = _db.Products
			.AsNoTracking()
			.Where(x => x.CategoryId == category.Id)
			.Skip((paginationModel.Page - 1) * paginationModel.Limit)
			.Take(paginationModel.Limit)
			.ToList();

		return products;
	}

	public ProductModel GetProduct(string id)
	{
		var product = _db.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);

		return product!;
	}

	public ProductResponseModel UpdateProduct(ProductModel requestModel)
	{
		ProductResponseModel responseModel = new();

		var product = GetProduct(requestModel.Id!);

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
		int result = _db.SaveChanges();

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

	public ProductResponseModel DeleteProduct(string id)
	{
		ProductResponseModel responseModel = new();
		var product = GetProduct(id);

		if (product is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Product not found.";
			responseModel.Data = null;
			return responseModel;
		}

		_db.Entry(product).State = EntityState.Deleted;
		int result = _db.SaveChanges();

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