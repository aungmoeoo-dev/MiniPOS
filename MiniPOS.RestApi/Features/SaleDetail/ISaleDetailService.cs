
namespace MiniPOS.RestApi.Features.SaleDetail
{
	public interface ISaleDetailService
	{
		Task<SaleDetailModel> GetSaleDetail(string id);
		Task<List<SaleDetailModel>> GetSaleDetails(SaleDetailPaginationModel paginationModel);
	}
}