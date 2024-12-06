using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniPOS.RestApi.Features.SaleDetail;

[Route("api/[controller]")]
[ApiController]
public class SaleDetailController : ControllerBase
{
	private SaleDetailService _saleDetailService;

	public SaleDetailController()
	{
		_saleDetailService = new SaleDetailService();
	}

	[HttpGet]
	public async Task<IActionResult> GetSaleDetails([FromQuery] SaleDetailPaginationModel paginationModel)
	{
		var saleDetails = await _saleDetailService.GetSaleDetails(paginationModel);

		return Ok(saleDetails);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetSaleDetail(string id)
	{
		var saleDetail = await _saleDetailService.GetSaleDetail(id);

		if (saleDetail is null) return BadRequest(saleDetail);

		return Ok(saleDetail);
	}
}
