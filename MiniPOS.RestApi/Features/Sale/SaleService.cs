using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MiniPOS.RestApi.Features.SaleDetail;
using MiniPOS.RestApi.Shared;
using MiniPOS.RestApi.Shared.Model;
using System.Collections.Generic;
using System.Transactions;

namespace MiniPOS.RestApi.Features.Sale;

public class SaleService : ISaleService
{
	private AppDbContext _db;

	public SaleService()
	{
		_db = new AppDbContext();
	}

	public async Task<SaleResponseModel> CreateSale(List<SaleDetailModel> saleDetails)
	{
		SaleResponseModel responseModel = new();

		string saleId = Guid.NewGuid().ToString();

		foreach (var saleDetail in saleDetails)
		{
			var product = await _db.Products
				.FirstOrDefaultAsync(product => product.Id == saleDetail.ProductId);

			if (product is null)
			{
				responseModel.IsSuccessful = false;
				responseModel.Data = null;

				return responseModel;
			}

			saleDetail.Id = Guid.NewGuid().ToString();
			saleDetail.SaleId = saleId;
			saleDetail.Price = product.Price;
			saleDetail.DiscountRate = product.DiscountRate;

			decimal beforeDiscountAmount = saleDetail.Price * saleDetail.Quantity;
			decimal afterDiscountAmount =
				beforeDiscountAmount - (beforeDiscountAmount * saleDetail.DiscountRate / 100);
			saleDetail.TotalAmount = afterDiscountAmount;
		}

		var transaction = await _db.Database.BeginTransactionAsync();

		decimal saleTotalAmount = default;

		try
		{
			foreach (var saleDetail in saleDetails)
			{
				saleTotalAmount += saleDetail.TotalAmount;
				_db.SaleDetails.Add(saleDetail);
				await _db.SaveChangesAsync();
			}

			SaleModel sale = new()
			{
				Id = saleId,
				TotalAmount = saleTotalAmount,
				SaleStatus = "paid",
				CreatedTime = DateTime.UtcNow,
			};
			_db.Sales.Add(sale);
			await _db.SaveChangesAsync();

			transaction.Commit();

			responseModel.IsSuccessful = true;
			responseModel.Data = sale;
		}
		catch (Exception ex)
		{
			responseModel.IsSuccessful = false;
			responseModel.Data = null;

			Console.WriteLine(ex.Message);
			transaction.Rollback();
		}

		return responseModel;

	}

	public async Task<List<SaleModel>> GetSales(PaginationModel paginationModel)
	{
		List<SaleModel> list = new();

		if (paginationModel.Page != 0 && paginationModel.Limit != 0)
		{
			list = await _db.Sales
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToListAsync();

			return list;
		}

		list = await _db.Sales.ToListAsync();
		return list;
	}

	public async Task<SaleModel> GetSale(string id)
	{
		var sale = await _db.Sales.FirstOrDefaultAsync(x => x.Id == id);

		return sale;
	}
}