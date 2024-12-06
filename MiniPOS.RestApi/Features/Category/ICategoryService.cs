using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Category
{
	public interface ICategoryService
	{
		Task<CategoryResponseModel> CreateCategory(CategoryModel requestModel);
		Task<CategoryResponseModel> DeleteCategory(string categoryName);
		Task<List<CategoryModel>> GetCategories(PaginationModel paginationModel);
		Task<CategoryModel> GetCategory(string categoryName);
		Task<CategoryResponseModel> UpdateCategory(CategoryModel requestModel);
	}
}