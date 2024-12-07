using Microsoft.EntityFrameworkCore;
using MiniPOS.RestApi.Shared;

namespace MiniPOS.RestApi.Features.SaleDetail;

public class SaleDetailService : ISaleDetailService
{
	private AppDbContext _db;

	public SaleDetailService()
	{
		_db = new AppDbContext();
	}

	public List<SaleDetailModel> GetSaleDetails(SaleDetailPaginationModel paginationModel)
	{
		List<SaleDetailModel> list = new();

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.ProductId is null
			&& paginationModel.SaleId is null)
		{
			list = _db.SaleDetails.ToList();
			return list;
		}

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.ProductId is not null
			&& paginationModel.SaleId is null)
		{
			list = _db.SaleDetails
				.Where(x => x.ProductId == paginationModel.ProductId)
				.ToList();
			return list;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.ProductId is null
			&& paginationModel.SaleId is null)
		{
			list = _db.SaleDetails
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToList();
			return list;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.ProductId is null
			&& paginationModel.SaleId is not null)
		{
			list = _db.SaleDetails
				.Where(x => x.SaleId == paginationModel.SaleId)
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToList();

			return list;
		}

		list = _db.SaleDetails
				.Where(x => x.ProductId == paginationModel.ProductId)
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToList();
		return list;

	}

	public SaleDetailModel GetSaleDetail(string id)
	{
		var saleDetail = _db.SaleDetails.FirstOrDefault(x => x.Id == id);

		return saleDetail;
	}
}
