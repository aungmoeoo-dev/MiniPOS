using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MiniPOS.RestApi.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace MiniPOS.RestApi.Features.Product;

[Table("TBL_Product")]
public class ProductModel
{
	[Key]
	[Column("ProductId")]
	public string? Id { get; set; }
	[Column("ProductName")]
	public string? Name { get; set; }
	[Column("ProductCategoryId")]
	public string? CategoryId { get; set; }
	[Column("ProductPrice")]
	public decimal Price { get; set; }
	[Column("ProductDiscountRate")]
	public decimal DiscountRate { get; set; }
}

public class ProductResponseModel
{
	public bool IsSuccessful { get; set; }
	public string? Message { get; set; }
	public ProductModel? Data { get; set; }
}

public class ProductPaginationModel : PaginationModel
{
	[FromQuery(Name = "Category")]
	public string? CategoryName { get; set; }
}