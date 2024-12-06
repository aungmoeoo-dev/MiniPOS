using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MiniPOS.RestApi.Features.Category;

[Table("TBL_Category")]
public class CategoryModel
{
	[Key]
	[Column("CategoryId")]
	public string? Id { get; set; }
	[Column("CategoryName")]
	public string? Name { get; set; }
	[Column("CategoryDescription")]
	public string? Description { get; set; }
}

public class CategoryResponseModel
{
	public bool IsSuccessful { get; set; }
	public string? Message { get; set; }
	public CategoryModel? Data { get; set; }
}