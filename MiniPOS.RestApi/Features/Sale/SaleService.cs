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

	public SaleResponseModel CreateSale(List<SaleDetailModel> saleDetails)
	{
		SaleResponseModel responseModel = new();

		string saleId = Guid.NewGuid().ToString();

		foreach (var saleDetail in saleDetails)
		{
			var product =_db.Products
				.FirstOrDefault(product => product.Id == saleDetail.ProductId);

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

		var transaction = _db.Database.BeginTransaction();

		decimal saleTotalAmount = default;

		try
		{
			foreach (var saleDetail in saleDetails)
			{
				saleTotalAmount += saleDetail.TotalAmount;
				_db.SaleDetails.Add(saleDetail);
				_db.SaveChanges();
			}

			SaleModel sale = new()
			{
				Id = saleId,
				TotalAmount = saleTotalAmount,
				SaleStatus = "paid",
				CreatedTime = DateTime.UtcNow,
			};
			_db.Sales.Add(sale);
			_db.SaveChanges();

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

	public List<SaleModel> GetSales(PaginationModel paginationModel)
	{
		List<SaleModel> list = new();

		if (paginationModel.Page != 0 && paginationModel.Limit != 0)
		{
			list = _db.Sales
				.Skip((paginationModel.Page - 1) * paginationModel.Limit)
				.Take(paginationModel.Limit)
				.ToList();

			return list;
		}

		list = _db.Sales.ToList();
		return list;
	}

	public SaleModel GetSale(string id)
	{
		var sale = _db.Sales.FirstOrDefault(x => x.Id == id);

		return sale;
	}
}