using MiniPOS.RestApi.Features.SaleDetail;
using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Sale
{
	public interface ISaleService
	{
		Task<SaleResponseModel> CreateSale(List<SaleDetailModel> saleDetails);
		Task<SaleModel> GetSale(string id);
		Task<List<SaleModel>> GetSales(PaginationModel paginationModel);
	}
}