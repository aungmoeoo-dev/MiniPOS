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

	public SaleResponseModel CreateSale(List<SaleDetailPublicRequestModel> saleDetails)
	{
		SaleResponseModel responseModel = new();
		List<SaleDetailModel> saleDetailList = new ();

		string saleId = Guid.NewGuid().ToString();

		for (var i = 0; i < saleDetails.Count; i++)
		{
			var saleDetail = saleDetails[i];

			var product = _db.Products
				.FirstOrDefault(product => product.Code == saleDetail.ProductCode);

			if (product is null)
			{
				responseModel.IsSuccessful = false;
				responseModel.Message = "Product not found.";
				responseModel.Data = null;
				return responseModel;
			}

			if (saleDetail.Quantity is null)
			{
				responseModel.IsSuccessful = false;
				responseModel.Message = "Required info not provided.";
				responseModel.Data = null;
				return responseModel;
			}

			product.Quantity -= (decimal) saleDetail.Quantity;
			_db.Entry(product).State = EntityState.Modified;
			_db.SaveChanges();

			SaleDetailModel saleDetailModel = new()
			{
				Id = Guid.NewGuid().ToString(),
				ProductId = product.Id,
				SaleId = saleId,
				Price = product.Price,
			};
			saleDetailList.Add(saleDetailModel);
			
		}

		decimal saleTotalAmount = default;

		foreach (var saleDetail in saleDetailList)
		{
			saleTotalAmount += saleDetail.Price * saleDetail.Quantity;
			_db.SaleDetails.Add(saleDetail);
			_db.SaveChanges();
		}

		var currentSaleIndex = _db.Sales.Count();
		

		SaleModel sale = new()
		{
			Id = saleId,
			VoucherId = currentSaleIndex.ToString(),
			TotalAmount = saleTotalAmount,
			CreatedTime = DateTime.UtcNow,
		};
		_db.Sales.Add(sale);
		_db.SaveChanges();

		responseModel.IsSuccessful = true;
		responseModel.Data = sale;

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

	public SaleModel GetSale(string voucherId)
	{
		var sale = _db.Sales.FirstOrDefault(x => x.VoucherId == voucherId);

		return sale;
	}
}