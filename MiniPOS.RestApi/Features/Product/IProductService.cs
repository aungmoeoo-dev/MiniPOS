
namespace MiniPOS.RestApi.Features.Product
{
	public interface IProductService
	{
		Task<ProductResponseModel> CreateProduct(ProductModel requestModel);
		Task<ProductResponseModel> DeleteProduct(string id);
		Task<ProductModel> GetProduct(string id);
		Task<List<ProductModel>> GetProducts(ProductPaginationModel paginationModel);
		Task<ProductResponseModel> UpdateProduct(ProductModel requestModel);
	}
}