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
	public async Task<IActionResult> CreateSale([FromBody] List<SaleDetailModel> saleDetails)
	{
		var responseModel = await _saleService.CreateSale(saleDetails);

		if (!responseModel.IsSuccessful) return BadRequest(responseModel);

		return Ok(responseModel);
	}

	[HttpGet]
	public async Task<IActionResult> GetSales([FromQuery] PaginationModel paginationModel)
	{
		var sales = await _saleService.GetSales(paginationModel);

		return Ok(sales);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetSale(string id)
	{
		var sale = await _saleService.GetSale(id);

		if (sale is null) return NotFound(sale);

		return Ok(sale);
	}
}
