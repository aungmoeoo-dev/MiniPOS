using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniPOS.RestApi.Features.SaleDetail;
using MiniPOS.RestApi.Shared.Model;

namespace MiniPOS.RestApi.Features.Sale;

[Route("api/[controller]")]
[ApiController]
public class SaleController : ControllerBase
{
	private ISaleService _saleService;

	public SaleController()
	{
		_saleService = new SaleService();
	}

	[HttpPost]
	public IActionResult CreateSale([FromBody] List<SaleDetailModel> saleDetails)
	{
		var responseModel = _saleService.CreateSale(saleDetails);

		if (!responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}

	[HttpGet]
	public IActionResult GetSales([FromQuery] PaginationModel paginationModel)
	{
		var sales = _saleService.GetSales(paginationModel);

		return Ok(sales);
	}

	[HttpGet("{id}")]
	public IActionResult GetSale(string id)
	{
		var sale = _saleService.GetSale(id);

		if (sale is null) return NotFound(sale);

		return Ok(sale);
	}
}
