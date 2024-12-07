
namespace MiniPOS.RestApi.Features.Product
{
	public interface IProductService
	{
		ProductResponseModel CreateProduct(ProductModel requestModel);
		ProductResponseModel DeleteProduct(string id);
		ProductModel GetProduct(string id);
		List<ProductModel> GetProducts(ProductPaginationModel paginationModel);
		ProductResponseModel UpdateProduct(ProductModel requestModel);
	}
}