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
	[Column("CategoryId")]
	public string? CategoryId { get; set; }
	[Column("ProductCode")]
	public string? Code { get; set; }
	[Column("ProductName")]
	public string? Name { get; set; }
	[Column("ProductPrice")]
	public decimal Price { get; set; }
	[Column("ProductQuantity")]
	public decimal Quantity {  get; set; }
}

public class ProductPublicRequestModel
{
	[Column("CategoryCode")]
	public string? CategoryCode { get; set; }
	[Column("ProductCode")]
	public string? Code { get; set; }
	[Column("ProductName")]
	public string? Name { get; set; }
	[Column("ProductPrice")]
	public decimal? Price { get; set; }
	[Column("ProductQuantity")]
	public decimal? Quantity { get; set; }
}

public class ProductResponseModel
{
	public bool IsSuccessful { get; set; }
	public string? Message { get; set; }
	public ProductModel? Data { get; set; }
}

public class ProductPaginationModel : PaginationModel
{
	[FromQuery(Name = "CategoryCode")]
	public string? CategoryCode { get; set; }
}