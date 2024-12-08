using Microsoft.AspNetCore.Mvc;
using MiniPOS.RestApi.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniPOS.RestApi.Features.SaleDetail;

[Table("TBL_SaleDetail")]
public class SaleDetailModel
{
	[Key]
	[Column("SaleDetailId")]
	public string? Id { get; set; }
	[Column("ProductId")]
	public string? ProductId { get; set; }
	[Column("SaleId")]
	public string? SaleId { get; set; }
	[Column("SaleDetailPrice")]
	public decimal Price { get; set; }
	[Column("SaleDetailQuantity")]
	public decimal Quantity { get; set; }
}

public class SaleDetailPublicRequestModel
{
	public string? ProductCode { get; set; }
	public decimal? Quantity { get; set; }
}

public class SaleDetailPaginationModel : PaginationModel
{
	[FromQuery(Name = "productId")]
	public string? ProductId { get; set; }

	[FromQuery(Name = "saleId")]
	public string? SaleId { get; set; }
}