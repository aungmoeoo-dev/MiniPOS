using MiniPOS.RestApi.Features.SaleDetail;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniPOS.RestApi.Features.Sale;

[Table("TBL_Sale")]
public class SaleModel
{
	[Key]
	[Column("SaleId")]
	public string? Id { get; set; }
	[Column("SaleStatus")]
	public string? SaleStatus { get; set; }
	[Column("SaleTotalAmount")]
	public decimal TotalAmount { get; set; }
	[Column("SaleCreatedTime")]
	public DateTime CreatedTime { get; set; }
}

public class SaleResponseModel
{
	public bool IsSuccessful { get; set; }
	public SaleModel? Data { get; set; }
}