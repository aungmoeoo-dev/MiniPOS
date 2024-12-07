using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Category
{
	public interface ICategoryService
	{
		CategoryResponseModel CreateCategory(CategoryModel requestModel);
		CategoryResponseModel DeleteCategory(string categoryName);
		List<CategoryModel> GetCategories(PaginationModel paginationModel);
		CategoryModel GetCategory(string categoryName);
		CategoryResponseModel UpdateCategory(CategoryModel requestModel);
	}
}