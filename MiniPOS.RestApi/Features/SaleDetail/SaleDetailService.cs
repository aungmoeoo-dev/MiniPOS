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

	public async Task<List<SaleDetailModel>> GetSaleDetails(SaleDetailPaginationModel paginationModel)
	{
		List<SaleDetailModel> list = new();

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.ProductId is null
			&& paginationModel.SaleId is null)
		{
			list = await _db.SaleDetails.ToListAsync();
			return list;
		}

		if (paginationModel.Page == 0
			&& paginationModel.Limit == 0
			&& paginationModel.ProductId is not null
			&& paginationModel.SaleId is null)
		{
			list = await _db.SaleDetails
				.Where(x => x.ProductId == paginationModel.ProductId)
				.ToListAsync();
			return list;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.ProductId is null
			&& paginationModel.SaleId is null)
		{
			list = await _db.SaleDetails
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToListAsync();
			return list;
		}

		if (paginationModel.Page != 0
			&& paginationModel.Limit != 0
			&& paginationModel.ProductId is null
			&& paginationModel.SaleId is not null)
		{
			list = await _db.SaleDetails
				.Where(x => x.SaleId == paginationModel.SaleId)
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToListAsync();

			return list;
		}

		list = await _db.SaleDetails
				.Where(x => x.ProductId == paginationModel.ProductId)
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToListAsync();
		return list;

	}

	public async Task<SaleDetailModel> GetSaleDetail(string id)
	{
		var saleDetail = await _db.SaleDetails.FirstOrDefaultAsync(x => x.Id == id);

		return saleDetail;
	}
}
