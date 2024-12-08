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

	public ProductResponseModel CreateProduct(ProductPublicRequestModel requestModel)
	{
		ProductResponseModel responseModel = new();

		if (requestModel.Price is not null && decimal.IsNegative((decimal) requestModel.Price))
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Price cannot be negative.";
			responseModel.Data = null;
			return responseModel;
		}

		if (requestModel.Quantity is not null && decimal.IsNegative((decimal)requestModel.Quantity))
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Quantity cannot be negative.";
			responseModel.Data = null;
			return responseModel;
		}

		if (string.IsNullOrEmpty(requestModel.Code))
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Required info not Provided.";
			responseModel.Data = null;
			return responseModel;
		}

		if (string.IsNullOrEmpty(requestModel.CategoryCode))
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Required info not Provided.";
			responseModel.Data = null;
			return responseModel;
		}

		var category = _db.Categories.FirstOrDefault(x => x.Code == requestModel.CategoryCode);
		if (category is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Category not found.";
			responseModel.Data = null;
			return responseModel;
		}

		ProductModel productModel = new()
		{
			Id = Guid.NewGuid().ToString(),
			CategoryId = category.Id,
			Code = requestModel.Code,
			Name = requestModel.Name,
			Price = (decimal)requestModel.Price,
			Quantity = (decimal)requestModel.Quantity,
		};

		_db.Products.Add(productModel);
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

		responseModel.Data = productModel;
		return responseModel;
	}

	public List<ProductModel> GetProducts(ProductPaginationModel paginationModel)
	{
		List<ProductModel> products;

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.CategoryCode is null)
		{
			products = _db.Products.ToList();

			return products;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.CategoryCode is null)
		{
			products = GetProductsByPagination(paginationModel);

			return products;
		}

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.CategoryCode is not null)
		{
			products = GetProductsByCategory(paginationModel.CategoryCode);

			return products;
		}

		products = GetProductsByCategoryPagination(paginationModel);
		return products;
	}

	private List<ProductModel> GetProductsByCategory(string categoryCode)
	{
		List<ProductModel> products = new();

		var category = _db.Categories
			.AsNoTracking()
			.FirstOrDefault(x => x.Code == categoryCode);

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
			.FirstOrDefault(x => x.Code == paginationModel.CategoryCode);

		if (category is null) return products;

		products = _db.Products
			.AsNoTracking()
			.Where(x => x.CategoryId == category.Id)
			.Skip((paginationModel.Page - 1) * paginationModel.Limit)
			.Take(paginationModel.Limit)
			.ToList();

		return products;
	}

	public ProductModel GetProduct(string productCode)
	{
		var product = _db.Products.AsNoTracking().FirstOrDefault(x => x.Code == productCode);

		return product!;
	}

	public ProductResponseModel UpdateProduct(ProductPublicRequestModel requestModel)
	{
		ProductResponseModel responseModel = new();

		var product = GetProduct(requestModel.Code!);

		if (product is null)
		{
			responseModel.IsSuccessful = false;
			responseModel.Message = "Product not found.";
			responseModel.Data = null;
			return responseModel;
		}

		if (requestModel.CategoryCode is not null)
		{
			var category = _db.Categories.FirstOrDefault(x => x.Code == requestModel.CategoryCode);
			if (category is null)
			{
				responseModel.IsSuccessful = false;
				responseModel.Message = "Category not found.";
				responseModel.Data = null;
				return responseModel;
			}

			product.CategoryId = category.Id;
		}

		if (requestModel.Code is not null)
		{
			product.Code = requestModel.Code;
		}

		if (requestModel.Name is not null)
		{
			product.Name = requestModel.Name;
		}

		if (requestModel.Price is not null && !decimal.IsNegative((decimal) requestModel.Price))
		{
			product.Price = (decimal) requestModel.Price;
		}

		if (requestModel.Quantity is not null &&  !decimal.IsNegative((decimal) requestModel.Quantity))
		{
			product.Quantity = (decimal) requestModel.Quantity;
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

		responseModel.Data = product;
		return responseModel;
	}

	public ProductResponseModel DeleteProduct(string productCode)
	{
		ProductResponseModel responseModel = new();
		var product = GetProduct(productCode);

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