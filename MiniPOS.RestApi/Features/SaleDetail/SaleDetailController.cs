using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniPOS.RestApi.Features.SaleDetail;

[Route("api/[controller]")]
[ApiController]
public class SaleDetailController : ControllerBase
{
	private ISaleDetailService _saleDetailService;

	public SaleDetailController()
	{
		_saleDetailService = new SaleDetailService();
	}

	[HttpGet]
	public IActionResult GetSaleDetails([FromQuery] SaleDetailPaginationModel paginationModel)
	{
		var saleDetails = _saleDetailService.GetSaleDetails(paginationModel);

		return Ok(saleDetails);
	}

	[HttpGet("{id}")]
	public IActionResult GetSaleDetail(string id)
	{
		var saleDetail = _saleDetailService.GetSaleDetail(id);

		if (saleDetail is null) return BadRequest(saleDetail);

		return Ok(saleDetail);
	}
}
