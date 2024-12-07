
namespace MiniPOS.RestApi.Features.SaleDetail
{
	public interface ISaleDetailService
	{
		SaleDetailModel GetSaleDetail(string id);
		List<SaleDetailModel> GetSaleDetails(SaleDetailPaginationModel paginationModel);
	}
}