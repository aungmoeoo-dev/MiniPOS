using MiniPOS.RestApi.Features.SaleDetail;
using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Sale
{
	public interface ISaleService
	{
		SaleResponseModel CreateSale(List<SaleDetailModel> saleDetails);
		SaleModel GetSale(string id);
		List<SaleModel> GetSales(PaginationModel paginationModel);
	}
}